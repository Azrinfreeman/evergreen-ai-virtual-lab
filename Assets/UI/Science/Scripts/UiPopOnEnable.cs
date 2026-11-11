using System.Collections;
using UnityEngine;

/// <summary>
/// Plays a short scale pop each time this object becomes active (for example an ItemSlot tick).
/// Scale only, so layout and colliders are unaffected. Shows the final state at once when
/// UiMotion.Reduced is on.
/// </summary>
public sealed class UiPopOnEnable : MonoBehaviour
{
    [SerializeField, Range(0.2f, 1f)] private float startScale = 0.6f;
    [SerializeField, Range(1f, 1.3f)] private float overshoot = 1.08f;
    [SerializeField, Range(0.05f, 0.4f)] private float duration = 0.16f;

    private Vector3 restScale;
    private bool restCaptured;
    private Coroutine running;

    private void OnEnable()
    {
        if (!restCaptured)
        {
            restScale = transform.localScale;
            restCaptured = true;
        }
        transform.localScale = restScale;
        if (UiMotion.Reduced) return;
        running = StartCoroutine(Pop());
    }

    private void OnDisable()
    {
        if (running != null) StopCoroutine(running);
        running = null;
        if (restCaptured) transform.localScale = restScale;
    }

    private IEnumerator Pop()
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = Mathf.Clamp01(t / duration);
            // 0 -> 0.7 rises from startScale to overshoot, then settles to 1.
            float s = k < 0.7f
                ? Mathf.Lerp(startScale, overshoot, k / 0.7f)
                : Mathf.Lerp(overshoot, 1f, (k - 0.7f) / 0.3f);
            transform.localScale = restScale * s;
            yield return null;
        }
        transform.localScale = restScale;
        running = null;
    }
}
