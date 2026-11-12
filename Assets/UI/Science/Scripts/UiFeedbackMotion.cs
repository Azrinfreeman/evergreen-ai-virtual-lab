using System.Collections;
using UnityEngine;

/// <summary>
/// One-shot feedback on the object it is attached to: Pop (correct, step done) and Wobble (wrong).
/// Uses scale and rotation only, so Layout Groups and colliders are not affected by position changes.
/// Does nothing when UiMotion.Reduced is on.
/// </summary>
public sealed class UiFeedbackMotion : MonoBehaviour
{
    [Header("Pop")]
    [SerializeField, Range(1f, 1.3f)] private float popScale = 1.04f;
    [SerializeField, Range(0.05f, 0.4f)] private float popDuration = 0.16f;

    [Header("Wobble")]
    [SerializeField, Range(0f, 6f)] private float wobbleDegrees = 1.5f;
    [SerializeField, Range(0.05f, 0.4f)] private float wobbleDuration = 0.18f;

    private Coroutine running;
    private Vector3 restScale = Vector3.one;
    private Quaternion restRotation = Quaternion.identity;

    public void Pop() => Play(PopRoutine());

    public void Wobble() => Play(WobbleRoutine());

    private void Play(IEnumerator routine)
    {
        if (UiMotion.Reduced || !isActiveAndEnabled) return;
        if (running != null) { StopCoroutine(running); Restore(); }
        restScale = transform.localScale;
        restRotation = transform.localRotation;
        running = StartCoroutine(routine);
    }

    private IEnumerator PopRoutine()
    {
        for (float t = 0f; t < popDuration; t += Time.unscaledDeltaTime)
        {
            float bump = Mathf.Sin(Mathf.Clamp01(t / popDuration) * Mathf.PI);
            transform.localScale = restScale * (1f + (popScale - 1f) * bump);
            yield return null;
        }
        Restore();
    }

    private IEnumerator WobbleRoutine()
    {
        for (float t = 0f; t < wobbleDuration; t += Time.unscaledDeltaTime)
        {
            float k = Mathf.Clamp01(t / wobbleDuration);
            float angle = wobbleDegrees * Mathf.Sin(k * Mathf.PI * 4f) * (1f - k);
            transform.localRotation = restRotation * Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
        Restore();
    }

    private void Restore()
    {
        transform.localScale = restScale;
        transform.localRotation = restRotation;
        running = null;
    }

    private void OnDisable()
    {
        if (running != null) { StopCoroutine(running); Restore(); }
    }
}
