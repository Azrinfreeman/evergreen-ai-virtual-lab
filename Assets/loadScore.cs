using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class loadScore : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 10 points per question (see QuestionController.addingmark)
        int maxScore = QuestionList.instance.questionlist.Count * 10;
        GetComponent<TextMeshProUGUI>().text = ScoreController.instance.score + " / " + maxScore;
    }

    // Update is called once per frame
    void Update() { }
}
