using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Shows a small "Selesai" tick on a Dashboard activity hub station when its stage has been
/// locally completed. Read-only; it never writes progress. The id is set in the Inspector and
/// must match the UiProgressMarker that completes the stage, for example "a6.eksperimen".
/// </summary>
public sealed class UiProgressBadge : MonoBehaviour
{
    [Tooltip("Stage id: a{1-7}.belajar, a{1-7}.eksperimen or a{1-7}.ujidiri.")]
    [SerializeField] private string progressId;
    [SerializeField] private GameObject checkRoot;
    [SerializeField] private TMP_Text checkLabel;
    [Tooltip("Assigned in the scene; drives the chip's visibility so we never AddComponent at runtime.")]
    [SerializeField] private CanvasGroup group;

    private bool wasComplete;
    private Coroutine fade;

    private void Awake()
    {
        // Hide the whole chip (image + label) until its stage is complete.
        if (group != null)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    private void OnEnable()
    {
        Refresh(true);
    }

    /// <summary>Called by UiProgressBadgeGroup when the hub or its saved progress changes.</summary>
    public void Refresh(bool instant)
    {
        if (group == null) return;
        bool complete = UiProgress.IsComplete(progressId);
        if (complete == wasComplete && !instant) return;
        wasComplete = complete;
        if (checkRoot != null) checkRoot.SetActive(complete);
        if (checkLabel != null) checkLabel.gameObject.SetActive(complete);
        if (fade != null) { StopCoroutine(fade); fade = null; }
        float target = complete ? 1f : 0f;
        group.alpha = (instant || UiMotion.Reduced) ? target : group.alpha;
        if (!instant && !UiMotion.Reduced && isActiveAndEnabled)
            fade = StartCoroutine(FadeTo(target, 0.12f));
    }

    private IEnumerator FadeTo(float target, float duration)
    {
        float from = group.alpha;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            group.alpha = Mathf.Lerp(from, target, t / duration);
            yield return null;
        }
        group.alpha = target;
        fade = null;
    }
}
