using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController instance;

    public int score;

    public TextMeshProUGUI textScore;

    [Header("Feedback (optional)")]
    [SerializeField] private UiFeedbackMotion coinMotion;
    [SerializeField, Range(0.1f, 0.8f)] private float countDuration = 0.4f;

    private Coroutine counting;

    void Awake()
    {
        instance = this;
    }

    public void SetScore()
    {
        if (counting != null) StopCoroutine(counting);
        if (UiMotion.Reduced || !isActiveAndEnabled)
        {
            textScore.text = score.ToString();
            return;
        }
        if (coinMotion != null) coinMotion.Pop();
        counting = StartCoroutine(CountUp());
    }

    private IEnumerator CountUp()
    {
        int.TryParse(textScore.text, out int from);
        for (float t = 0f; t < countDuration; t += Time.unscaledDeltaTime)
        {
            textScore.text = Mathf.RoundToInt(Mathf.Lerp(from, score, t / countDuration)).ToString();
            yield return null;
        }
        textScore.text = score.ToString();
        counting = null;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
