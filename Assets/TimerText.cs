using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerText : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float elapsedTime = 0f;
    [Min(0)] public float timeMultiplier = 22f;
    private bool counting = true;

    public bool isMinute;
    public bool is45min;
    public bool isTimer;

    [Tooltip("Optional duration in simulated seconds. Zero preserves the legacy 30/45/60 duration.")]
    [SerializeField, Min(0)] private float durationSeconds;
    [Tooltip("Controls revealed only when this timer or temperature reaches its target.")]
    [SerializeField] private GameObject[] completionTargets = new GameObject[0];
    [SerializeField] private UnityEvent onCompleted = new UnityEvent();

    [Space]
    public float currentTemp = 35f;
    [Min(0)] public float speed = 5f;
    public float targetTemp = 37f;

    public bool IsComplete { get; private set; }
    public float Duration => durationSeconds > 0 ? durationSeconds : isMinute ? 60f : is45min ? 45f : 30f;

    private void Update()
    {
        if (!counting) return;

        if (isTimer)
        {
            // Clamp before formatting; a long frame must not skip the completion threshold.
            elapsedTime = Mathf.Min(Duration, elapsedTime + Time.deltaTime * Mathf.Max(0, timeMultiplier));
            if (timerText != null)
                timerText.text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(elapsedTime / 60f), Mathf.FloorToInt(elapsedTime % 60f));
            if (elapsedTime >= Duration) Complete();
        }
        else
        {
            currentTemp = Mathf.MoveTowards(currentTemp, targetTemp, Time.deltaTime * Mathf.Max(0, speed));
            if (timerText != null) timerText.text = Mathf.FloorToInt(currentTemp) + "°C";
            if (Mathf.Approximately(currentTemp, targetTemp)) Complete();
        }
    }

    private void Complete()
    {
        counting = false;
        IsComplete = true;
        foreach (GameObject target in completionTargets)
            if (target != null) target.SetActive(true);
        onCompleted.Invoke();
    }
}
