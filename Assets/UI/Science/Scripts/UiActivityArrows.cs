using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows the "Aktiviti sebelum / berikut" buttons only while an activity hub is on screen,
/// and disables the one that would do nothing at the first or last activity.
/// Navigation itself stays in MenuSlideController.
/// </summary>
public sealed class UiActivityArrows : MonoBehaviour
{
    [Tooltip("The Into/Content object that owns one child per activity (MenuSlideController).")]
    [SerializeField] private Transform activities;
    [SerializeField] private GameObject previousArrow;
    [SerializeField] private GameObject nextArrow;
    [Tooltip("Names of the child that holds an activity's hub screen.")]
    [SerializeField] private string[] hubChildNames = { "0", "bg" };

    private Button previousButton;
    private Button nextButton;

    private void Awake()
    {
        previousButton = previousArrow != null ? previousArrow.GetComponentInChildren<Button>(true) : null;
        nextButton = nextArrow != null ? nextArrow.GetComponentInChildren<Button>(true) : null;
    }

    private void Update()
    {
        if (activities == null) return;
        int active = -1;
        for (int i = 0; i < activities.childCount; i++)
        {
            if (activities.GetChild(i).gameObject.activeSelf) { active = i; break; }
        }
        bool onHub = active >= 0 && HubVisible(activities.GetChild(active));
        SetShown(previousArrow, onHub);
        SetShown(nextArrow, onHub);
        if (!onHub) return;
        if (previousButton != null) previousButton.interactable = active > 0;
        if (nextButton != null) nextButton.interactable = active < activities.childCount - 1;
    }

    private bool HubVisible(Transform activity)
    {
        foreach (string hubName in hubChildNames)
        {
            Transform hub = activity.Find(hubName);
            if (hub != null && hub.gameObject.activeInHierarchy) return true;
        }
        return false;
    }

    private static void SetShown(GameObject target, bool shown)
    {
        if (target != null && target.activeSelf != shown) target.SetActive(shown);
    }
}
