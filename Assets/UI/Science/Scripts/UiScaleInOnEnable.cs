using System.Collections;
using UnityEngine;

/// <summary>
/// Scales this object in from startScale to its rest scale each time it becomes active
/// (for example the Seterusnya button). Scale only; shows the final state at once when UiMotion.Reduced.
/// </summary>
public sealed class UiScaleInOnEnable : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1f)] private float startScale = 0.85f;
    [SerializeField, Range(0.05f, 0.5f)] private float duration = 0.2f;

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
        running = StartCoroutine(ScaleIn());
    }

    private void OnDisable()
    {
        if (running != null) StopCoroutine(running);
        running = null;
        if (restCaptured) transform.localScale = restScale;
    }

    private IEnumerator ScaleIn()
    {
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = 1f - Mathf.Clamp01(t / duration);
            float eased = 1f - k * k * k; // ease-out cubic
            transform.localScale = restScale * Mathf.Lerp(startScale, 1f, eased);
            yield return null;
        }
        transform.localScale = restScale;
        running = null;
    }
}
