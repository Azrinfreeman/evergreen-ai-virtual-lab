using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Evergreen.TutorDemo;
using UnityEngine;
using UnityEngine.UI;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField]
        private InputField inputField;

        [SerializeField]
        private Button button;

        [SerializeField]
        private ScrollRect scroll;

        [SerializeField]
        private RectTransform sent;

        [SerializeField]
        private RectTransform received;

        [SerializeField]
        [Tooltip("OpenAI model ID used by the science tutor. Requires access on the API account.")]
        private string model = "gpt-6-luna";

        public CheckBehaviour checkBehaviour;

        [SerializeField] private TMPro.TextMeshProUGUI statusLabel;
        private bool sending;

        [Header("Tutor source")]
        [SerializeField]
        [Tooltip("Auto: live OpenAI tutor when a key exists, scripted replies otherwise. See docs/tutorial/tutor-demo-mode.md.")]
        private TutorMode mode = TutorMode.Auto;

        [SerializeField]
        [Tooltip("Scripted replies used when the tutor runs without a key.")]
        private DemoTutorScript demoScript;

        [SerializeField]
        [Tooltip("Leave off for the shipped app. Turn on only to record footage; it hides the notice after scripted replies.")]
        private bool hideDemoNotice;

        [SerializeField]
        [Tooltip("Off (default): release player builds never call OpenAI and always use the scripted replies, so no key on a learner's machine is used. Turn on only for a build that talks to your own backend.")]
        private bool allowLiveInRelease;

        private bool usingDemo;
        private bool liveBlocked;
        private string pendingQuestion;

        private float height;
        public string[,] tablePrompts = new string[,]
        {
            {
                "defaultPrompt",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Don't answer question that are not scientific questions",
            },
            {
                "Dashboard",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Only explain about the list of content / activities in the game that the player can choose from Aktiviti 1 to Aktiviti 8, which are akt 1 = Apakah Stem, akt 2 = Apakah Teknologi Hijau, akt 3= Kenali Bakteria Baik, akt 4 = Teknologi Pengklonan, akt 5= Teknologi Hijau Dan Kesihatan, akt 6 = Teknologi Sel Bahan Bakar Mikrob dan akt 7 = Teknologi Filem Getah. Don't answer question that are not scientific questions",
            },
            {
                "Akt1",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 1 which is Apakah Stem in Science, and why stem as well. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. Don't answer question that are not scientific questions",
            },
            {
                "Akt2",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 2 which is Apakah Teknologi Hijau in Science, and why it is important. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. Don't answer question that are not scientific questions",
            },
            {
                "Akt3",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 3 which is Kenali Bakteria Baik in Science, and why it is important. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. There is also Makmal Maya, or Virtual Lab that player can try which is in 2d mode. Don't answer question that are not scientific questions",
            },
            {
                "Akt4",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 4 which is Teknologi Pengklonan in Science, and why it is important. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. There is also Makmal Maya, or Virtual Lab that player can try which is in 2d mode. Don't answer question that are not scientific questions",
            },
            {
                "Akt5",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 5 which is Teknologi Hijau Dan Kesihatan in Science, and why it is important. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. There is also Makmal Maya, or Virtual Lab that player can try which is in 2d mode. Don't answer question that are not scientific questions",
            },
            {
                "Akt6",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 6 which is Teknologi Sel Bahan Bakar Mikrob in Science, and why it is important. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. There is also Makmal Maya, or Virtual Lab that player can try which is in 2d mode. Don't answer question that are not scientific questions",
            },
            {
                "Akt7",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Explain the Aktiviti 7 which is Teknologi Filem Getah, or Rubber Film in Science, and why it is important. You can tell the player about the Uji Diri or test that they can try that the game developer put in the game which is multiple choice answer questions test. There is also Makmal Maya, or Virtual Lab that player can try which is in 2d mode. Don't answer question that are not scientific questions",
            },
            {
                "MM3",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Only explain about the Experiment 3 which is Penkulturan Bakteria, the objective of this experiment is to culture the bacteria using two different bateria, agar-agar nutrien. If you have any knowledge about this experiment feel free to explain to the player. Don't answer question that are not scientific questions",
            },
            {
                "MM4",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Only explain about the Experiment 4 which is Cloning or Pengklonan, the objective of this experiment is to TUJUAN EKSPERIMEN: Untuk memasukkan gen Protein Pendarfluor Hijau (Green Fluorescence Protein) ke dalam vektor plasmid (pUC19) menggunakan pencernaan enzim sekatan, pengikatan, transformasi dan pengesahan oleh PCR (polymerase chain reaction). If you have any knowledge about this experiment feel free to explain to the player. Don't answer question that are not scientific questions",
            },
            {
                "MM5",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Only explain about the Experiment 5 which is Testing of Mercuri, Pengujian Merkuri, the objective of Untuk analisis kehadiran bahan pencemar merkuri dalam produk kecantikan dan kesihatan .If you have any knowledge about this experiment feel free to explain to the player. Don't answer question that are not scientific questions",
            },
            {
                "MM6",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Only explain about the Experiment 6 which is Langkah-Langkah Pembuatan Mikrobial Fuel Cell (MFC) untuk Pelajar. The objective of this experiment is to guide students through the steps of creating a microbial fuel cell. If you have any knowledge about this experiment feel free to explain to the player. Don't answer question that are not scientific questions",
            },
            {
                "MM7",
                "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Only explain about the Experiment 7 which is Rubber film. If you have any knowledge about this experiment feel free to explain to the player. Don't answer question that are not scientific questions",
            },
        };

        static bool TryGetKeyById(string[,] t, string id, out string key)
        {
            for (int r = 0; r < t.GetLength(0); r++)
                if (string.Equals(t[r, 0], id, StringComparison.OrdinalIgnoreCase))
                {
                    key = t[r, 1];
                    return true;
                }

            key = null;
            return false;
        }

        // Key comes from the OPENAI_API_KEY environment variable; when it is unset the
        // OpenAI-Unity package falls back to ~/.openai/auth.json. Never hard-code keys here.
        public OpenAIApi openai = new OpenAIApi(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

        private List<ChatMessage> messages = new List<ChatMessage>();
        public string prompt =
            "Act as a scientist in a chat room and reply to the questions related to science only. Don't break character. Don't ever mention that you are an AI model. Don't answer question that are not scientific questions";

        private void Start()
        {
            button.onClick.AddListener(SendReply);
            ResolveTutorMode();

            // Call this to update the prompt based on CheckBehaviour immediately upon loading
            ChangePrompt();
        }

        void Update() { }

        public void ChangePrompt()
        {
            if (TryGetKeyById(tablePrompts, checkBehaviour.BehaviourName, out var key))
            {
                Debug.Log(key); // "shield"
                prompt = key;
            }
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private static bool LaunchedWithDemoFlag()
        {
            foreach (string arg in Environment.GetCommandLineArgs())
                if (string.Equals(arg, "-tutor-demo", StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        private void ResolveTutorMode()
        {
            // Release builds, and any player started with -tutor-demo (used for trailer capture),
            // never call OpenAI and always use the scripted replies.
            bool launchedDemo = LaunchedWithDemoFlag();
            bool releaseLocked = launchedDemo || (!Application.isEditor && !Debug.isDebugBuild && !allowLiveInRelease);
            bool wantDemo = releaseLocked || mode == TutorMode.Demo || (mode == TutorMode.Auto && !DemoTutorProvider.KeyAvailable());
            if (wantDemo && demoScript == null)
            {
                if (releaseLocked)
                {
                    // Never fall back to a live call in a release build.
                    Debug.LogError("Tutor mode: release build needs a DemoTutorScript on the ChatGPT component; tutor disabled.");
                    liveBlocked = true;
                    usingDemo = false;
                    return;
                }
                Debug.LogError("Tutor mode: Demo needs a DemoTutorScript assigned on the ChatGPT component; using live.");
                wantDemo = false;
            }
            usingDemo = wantDemo;
            Debug.Log(usingDemo
                ? (launchedDemo ? "Tutor mode: Demo (-tutor-demo)"
                    : releaseLocked ? "Tutor mode: Demo (release build)"
                    : mode == TutorMode.Demo ? "Tutor mode: Demo (forced)" : "Tutor mode: Demo (no key found)")
                : "Tutor mode: Live");
        }

        private async void SendReply()
        {
            if (liveBlocked)
                return;
            if (usingDemo)
                await SendReplyAsync(request =>
                    DemoTutorProvider.ReplyAsync(demoScript, checkBehaviour.BehaviourName, pendingQuestion));
            else
                await SendReplyAsync(request => openai.CreateChatCompletion(request));
        }

        // Keep the UI event entry point small and allow the request flow to be checked locally.
        private async Task SendReplyAsync(
            Func<CreateChatCompletionRequest, Task<CreateChatCompletionResponse>> requestCompletion)
        {
            if (sending || string.IsNullOrWhiteSpace(inputField.text)) return;
            sending = true;
            button.interactable = false;
            inputField.interactable = false;
            if (statusLabel != null) statusLabel.text = "Sedang menyediakan jawapan...";

            try
            {
                pendingQuestion = inputField.text.Trim();
                var newMessage = new ChatMessage() { Role = "user", Content = pendingQuestion };
                AppendMessage(newMessage);
                if (messages.Count == 0) newMessage.Content = prompt + "\n" + newMessage.Content;
                messages.Add(newMessage);
                inputField.text = "";

                var response = await requestCompletion(new CreateChatCompletionRequest()
                {
                    Model = model,
                    Messages = messages,
                    Temperature = null,
                    PresencePenalty = null,
                    FrequencyPenalty = null,
                });
                if (this == null) return;

                if (response.Error != null)
                {
                    if (statusLabel != null)
                        statusLabel.text = "Jawapan tidak dapat dimuatkan. Cuba lagi.";
                }
                else if (response.Choices != null && response.Choices.Count > 0
                    && !string.IsNullOrWhiteSpace(response.Choices[0].Message.Content))
                {
                    var message = response.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    messages.Add(message);
                    AppendMessage(message);
                    if (statusLabel != null)
                        statusLabel.text = usingDemo && !hideDemoNotice
                            ? demoScript.notice
                            : "Semak jawapan bersama bahan pembelajaran.";
                }
                else if (statusLabel != null)
                    statusLabel.text = "Tiada jawapan diterima. Cuba hantar soalan semula.";
            }
            catch (Exception)
            {
                if (this != null && statusLabel != null)
                    statusLabel.text = "Jawapan tidak dapat dimuatkan. Cuba lagi.";
            }
            finally
            {
                if (this != null)
                {
                    sending = false;
                    if (button != null) button.interactable = true;
                    if (inputField != null) inputField.interactable = true;
                }
            }
        }
    }
}
