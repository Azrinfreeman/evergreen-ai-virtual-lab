using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One pill per question. Shows the current question and, after each answer, "Betul" or "Salah"
/// from this session's real result. Nothing is saved. Pills beyond the question count are hidden.
/// </summary>
public sealed class QuizProgressPills : MonoBehaviour
{
    [SerializeField] private Image[] pills = new Image[0];
    [SerializeField] private TMP_Text[] labels = new TMP_Text[0];

    [Header("Colours (text stays readable on each)")]
    [SerializeField] private Color pendingColor = new Color32(220, 229, 239, 255);
    [SerializeField] private Color currentColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color correctColor = new Color32(22, 101, 52, 255);
    [SerializeField] private Color wrongColor = new Color32(185, 28, 28, 255);
    [SerializeField] private Color darkText = new Color32(11, 11, 59, 255);
    [SerializeField] private Color lightText = Color.white;

    private bool[] answered = new bool[0];

    public void Setup(int questionCount)
    {
        answered = new bool[pills.Length];
        for (int i = 0; i < pills.Length; i++)
        {
            bool used = i < questionCount;
            pills[i].gameObject.SetActive(used);
            if (used) Show(i, pendingColor, darkText, (i + 1).ToString(), FontStyles.Normal);
        }
    }

    public void SetCurrent(int index)
    {
        if (index < 0 || index >= pills.Length || answered[index]) return;
        Show(index, currentColor, darkText, (index + 1).ToString(), FontStyles.Bold);
    }

    public void Record(int index, bool correct)
    {
        if (index < 0 || index >= pills.Length) return;
        answered[index] = true;
        Show(index, correct ? correctColor : wrongColor, lightText,
            (index + 1) + (correct ? " Betul" : " Salah"), FontStyles.Bold);
    }

    private void Show(int i, Color fill, Color text, string label, FontStyles style)
    {
        pills[i].color = fill;
        labels[i].text = label;
        labels[i].color = text;
        labels[i].fontStyle = style;
    }
}
