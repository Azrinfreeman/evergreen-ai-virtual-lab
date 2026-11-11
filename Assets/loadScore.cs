using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class loadScore : MonoBehaviour
{
    [Tooltip("Optional line that receives a short message based on the real result.")]
    [SerializeField] private TextMeshProUGUI messageLabel;

    // Start is called before the first frame update
    void Start()
    {
        // 10 points per question (see QuestionController.addingmark)
        int questionCount = QuestionList.instance.questionlist.Count;
        int maxScore = questionCount * 10;
        int score = ScoreController.instance.score;
        int correct = score / 10;
        GetComponent<TextMeshProUGUI>().text =
            correct + " daripada " + questionCount + " betul (" + score + " / " + maxScore + " mata)";

        if (messageLabel == null) return;
        string message;
        if (correct >= questionCount) message = "Hebat, semua betul!";
        else if (correct * 2 >= questionCount) message = "Bagus! Semak soalan yang salah dan cuba lagi.";
        else message = "Baca semula teori, kemudian cuba lagi.";
        messageLabel.text = message;
    }

    // Update is called once per frame
    void Update() { }
}
