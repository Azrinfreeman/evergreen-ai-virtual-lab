using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    void Awake() { }

    public TextMeshProUGUI questionNumberTopLabel;
    public TextMeshProUGUI questionLabel;

    public TextMeshProUGUI langkahLabel;
    public List<Transform> answerBtn;
    public Transform avatarState;

    public Transform CongratPrompt;
    public int countQuestion;
    public int countStep;

    [Header("Answer presentation")]
    [SerializeField] private Color answerColor = new Color32(232, 239, 248, 255);
    [SerializeField] private Color correctAnswerColor = new Color32(221, 239, 225, 255);
    [SerializeField] private Color otherAnswerColor = new Color32(248, 231, 231, 255);
    [SerializeField] private TextMeshProUGUI feedbackLabel;
    [SerializeField] private QuizProgressPills progressPills;
    private bool answerLayoutDirty = true;
    private int pickedIndex = -1;

    void defaultAvatarFace()
    {
        avatarState.GetChild(0).gameObject.SetActive(true);
        avatarState.GetChild(1).gameObject.SetActive(false);
        avatarState.GetChild(2).gameObject.SetActive(false);
    }

    void AvatarSmile()
    {
        avatarState.GetChild(0).gameObject.SetActive(false);
        avatarState.GetChild(1).gameObject.SetActive(true);
        avatarState.GetChild(2).gameObject.SetActive(false);
    }

    void AvatarSad()
    {
        avatarState.GetChild(0).gameObject.SetActive(false);
        avatarState.GetChild(1).gameObject.SetActive(false);
        avatarState.GetChild(2).gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        countQuestion = 0;
        countStep = 0;
        if (progressPills != null) progressPills.Setup(QuestionList.instance.questionlist.Count);

        Debug.Log(QuestionList.instance.questionlist[countQuestion].question[countStep]);
        applyQuestion();
        applyAnswerOnButton();
    }

    private void applyQuestion()
    {
        questionNumberTopLabel.text =
            "Soalan " + (countQuestion + 1) + " daripada " + QuestionList.instance.questionlist.Count;
        questionLabel.text = QuestionList.instance.questionlist[countQuestion].question[0];
        pickedIndex = -1;
        if (progressPills != null) progressPills.SetCurrent(countQuestion);
    }

    private void applyAnswerOnButton()
    { //disable all buttons first
        answerLayoutDirty = true;
        /*
            switch (countStep)
            {
                case 0:
                    langkahLabel.text = "Langkah 1 : Maklumat diberi";
                    break;
                case 1:
                    langkahLabel.text = "Langkah 2 : Operasi";
                    break;
                case 2:
                    langkahLabel.text = "Langkah 3 : Selesaikan ";
                    break;
                case 3:
                    langkahLabel.text = "Langkah 4 : Semak";
                    break;
                case 4:
                    langkahLabel.text = "Jawapan : ";
                    break;
            }
          */for (int i = 0; i < answerBtn.Count; i++)
        {
            answerBtn[i].gameObject.SetActive(false);
        }
        //check for answes count
        for (
            int i = 0;
            i
                < QuestionList
                    .instance
                    .questionlist[countQuestion]
                    .QuestionOnAnswersBtn[countStep]
                    .answer
                    .Count;
            i++
        )
        {
            answerBtn[i].gameObject.SetActive(true);
        }
        //assign text answer on all buttons
        for (
            int i = 0;
            i
                < QuestionList
                    .instance
                    .questionlist[countQuestion]
                    .QuestionOnAnswersBtn[countStep]
                    .answer
                    .Count;
            i++
        )
        {
            answerBtn[i].GetComponent<Button>().onClick.RemoveAllListeners();
            int optionIndex = i;
            answerBtn[i].GetComponent<Button>().onClick.AddListener(() => pickedIndex = optionIndex);

            answerBtn[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = QuestionList
                .instance
                .questionlist[countQuestion]
                .QuestionOnAnswersBtn[countStep]
                .answer[i];

            if (
                QuestionList
                    .instance
                    .questionlist[countQuestion]
                    .QuestionOnAnswersBtn[countStep]
                    .answeOnBtn == i
            )
            {
                Debug.Log(
                    QuestionList
                        .instance
                        .questionlist[countQuestion]
                        .QuestionOnAnswersBtn[countStep]
                        .answeOnBtn
                );
                answerBtn[i].GetComponent<Button>().onClick.AddListener(() => AddMark());
                answerBtn[i].GetComponent<Button>().onClick.AddListener(() => NextStepping());
            }
            else
            {
                answerBtn[i].GetComponent<Button>().onClick.AddListener(() => IncorrectButton());
                answerBtn[i].GetComponent<Button>().onClick.AddListener(() => NextStepping());
            }
        }
    }

    void disableAllButtons(bool boolean)
    {
        if (!boolean && feedbackLabel != null)
            feedbackLabel.text = "Sedang menyemak jawapan...";
        for (int i = 0; i < answerBtn.Count; i++)
        {
            Button answerButton = answerBtn[i].GetComponent<Button>();
            // Keep the existing input lock and expose it to Selectable navigation and tinting.
            answerButton.interactable = boolean;
            answerButton.enabled = boolean;
        }
    }

    void showRealAnswer()
    {
        answerLayoutDirty = true;
        if (feedbackLabel != null) feedbackLabel.text = "Jawapan betul ditandakan di bawah.";
        int correctIndex = QuestionList
            .instance
            .questionlist[countQuestion]
            .QuestionOnAnswersBtn[countStep]
            .answeOnBtn;
        if (progressPills != null) progressPills.Record(countQuestion, pickedIndex == correctIndex);
        for (int i = 0; i < answerBtn.Count; i++)
        {
            bool picked = i == pickedIndex;
            UiFeedbackMotion motion = answerBtn[i].GetComponent<UiFeedbackMotion>();
            TextMeshProUGUI label = answerBtn[i].GetChild(0).GetComponent<TextMeshProUGUI>();
            if (i == correctIndex)
            {
                answerBtn[i].GetComponent<Image>().color = correctAnswerColor;
                label.text = (picked ? "Pilihan anda · Betul: " : "Betul: ") + label.text;
                if (motion != null) motion.Pop();
            }
            else
            {
                answerBtn[i].GetComponent<Image>().color = otherAnswerColor;
                if (picked)
                {
                    label.text = "Pilihan anda: " + label.text;
                    if (motion != null) motion.Wobble();
                }
            }
        }
    }

    void resetButton()
    {
        if (feedbackLabel != null) feedbackLabel.text = "";
        for (int i = 0; i < answerBtn.Count; i++)
        {
            answerBtn[i].GetComponent<Image>().color = answerColor;
        }
    }

    public void AddMark()
    {
        //turn everybutton to disable
        disableAllButtons(false);

        StartCoroutine(addingmark());
    }

    public void IncorrectButton()
    { //turn everybutton to disable
        disableAllButtons(false);

        StartCoroutine(incorrect());
    }

    IEnumerator incorrect()
    {
        yield return new WaitForSeconds(1);
        if (!GameObject.Find("salah").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("salah").GetComponent<AudioSource>().Play();
        }
        //sad
        AvatarSad();
    }

    IEnumerator addingmark()
    {
        yield return new WaitForSeconds(1);
        if (!GameObject.Find("yay").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("yay").GetComponent<AudioSource>().Play();
        }
        if (!GameObject.Find("claps").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("claps").GetComponent<AudioSource>().Play();
        }

        //smile
        AvatarSmile();

        //play animation add coin and change text
        //        GameObject.Find("Coin").GetComponent<Animator>().Play("addCoin");
        yield return new WaitForSeconds(1.3f);
        ScoreController.instance.score += 10;
        ScoreController.instance.SetScore();
    }

    IEnumerator nextStep()
    {
        //turn everybutton to red
        for (int i = 0; i < answerBtn.Count; i++)
        {
            answerBtn[i].GetComponent<Button>().enabled = false;
        }
        yield return new WaitForSeconds(1.2f);
        showRealAnswer();
        countStep++;
        yield return new WaitForSeconds(3);
        countQuestion++;
        if (countQuestion >= QuestionList.instance.questionlist.Count)
        {
            //countStep++;
            // applyQuestion();
            //  applyAnswerOnButton();
            CongratPrompt.gameObject.SetActive(true);
        }
        else
        {
            countStep = 0;
            applyQuestion();
            applyAnswerOnButton();
        }

        disableAllButtons(true);
        resetButton();
        defaultAvatarFace();
    }

    public void NextStepping()
    {
        StartCoroutine(nextStep());
    }

    // Update is called once per frame
    private void OnRectTransformDimensionsChange()
    {
        answerLayoutDirty = true;
    }

    private void LateUpdate()
    {
        if (!answerLayoutDirty || answerBtn == null) return;
        answerLayoutDirty = false;
        Canvas.ForceUpdateCanvases();
        foreach (Transform answer in answerBtn)
        {
            if (answer == null || !answer.gameObject.activeSelf) continue;
            LayoutElement layout = answer.GetComponent<LayoutElement>();
            TextMeshProUGUI label = answer.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (layout == null || label == null) continue;
            layout.minHeight = Mathf.Max(88f,
                label.GetPreferredValues(label.text, label.rectTransform.rect.width, 0).y + 24f);
            layout.preferredHeight = layout.minHeight;
        }
    }

    public void startQuestion()
    {
        //questionLabel.text = QuestionList.instance.question[countQuestion];
    }
}
