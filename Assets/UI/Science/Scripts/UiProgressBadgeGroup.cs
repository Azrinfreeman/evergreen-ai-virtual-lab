using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Refreshes every UiProgressBadge under its owner whenever the scene loads or the Into
/// container becomes active after returning from a lesson, experiment, or quiz.
/// Attach it to the Dashboard's `Container/Into` object so the badges show current
/// completion the moment a hub is visible again.
/// </summary>
public sealed class UiProgressBadgeGroup : MonoBehaviour
{
    private UiProgressBadge[] badges;

    private void OnEnable()
    {
        Refresh();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Refresh();
    }

    public void Refresh()
    {
        badges = GetComponentsInChildren<UiProgressBadge>(true);
        if (badges == null) return;
        foreach (var badge in badges) badge.Refresh(true);
    }
}
