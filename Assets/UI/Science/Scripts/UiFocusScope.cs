using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Owns initial focus and optionally blocks underlying UI while a dialog is open.</summary>
public sealed class UiFocusScope : MonoBehaviour
{
    [SerializeField] private Selectable initialSelection;
    [Tooltip("Underlying screen groups blocked while this dialog is active. Leave empty for ordinary pages.")]
    [SerializeField] private CanvasGroup[] blockedGroups = new CanvasGroup[0];
    [Tooltip("For persistent dialogs: block other root canvases in the currently loaded scene.")]
    [SerializeField] private bool blockOtherCanvases;
    [Tooltip("Optional close/cancel action invoked by Escape while this dialog is active.")]
    [SerializeField] private Button cancelButton;

    private GameObject previousSelection;
    private readonly List<CanvasGroup> activeBlockedGroups = new List<CanvasGroup>();
    private bool[] previousInteractable;
    private bool[] previousRaycasts;
    private Coroutine selectRoutine;

    private void OnEnable()
    {
        previousSelection = EventSystem.current != null
            ? EventSystem.current.currentSelectedGameObject : null;
        activeBlockedGroups.Clear();
        foreach (CanvasGroup group in blockedGroups)
            if (group != null && !activeBlockedGroups.Contains(group))
                activeBlockedGroups.Add(group);
        if (blockOtherCanvases)
        {
            Canvas owner = GetComponentInParent<Canvas>();
            foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
            {
                if (!canvas.isRootCanvas || canvas == owner) continue;
                CanvasGroup group = canvas.GetComponent<CanvasGroup>();
                if (group == null) group = canvas.gameObject.AddComponent<CanvasGroup>();
                if (!activeBlockedGroups.Contains(group)) activeBlockedGroups.Add(group);
            }
        }
        previousInteractable = new bool[activeBlockedGroups.Count];
        previousRaycasts = new bool[activeBlockedGroups.Count];
        for (int i = 0; i < activeBlockedGroups.Count; i++)
        {
            CanvasGroup group = activeBlockedGroups[i];
            previousInteractable[i] = group.interactable;
            previousRaycasts[i] = group.blocksRaycasts;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        selectRoutine = StartCoroutine(SelectAfterLayout());
    }

    private IEnumerator SelectAfterLayout()
    {
        // Existing callbacks may activate this page and its controls in the same frame.
        yield return null;
        selectRoutine = null;
        if (EventSystem.current != null && initialSelection != null
            && initialSelection.IsActive() && initialSelection.IsInteractable())
            EventSystem.current.SetSelectedGameObject(initialSelection.gameObject);
    }

    private void Update()
    {
        if (!blockOtherCanvases || EventSystem.current == null) return;
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (initialSelection != null && initialSelection.IsActive() && initialSelection.IsInteractable()
            && (selected == null || !selected.transform.IsChildOf(transform)))
            EventSystem.current.SetSelectedGameObject(initialSelection.gameObject);

#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKeyDown(KeyCode.Escape) && cancelButton != null
            && cancelButton.IsActive() && cancelButton.IsInteractable())
            cancelButton.onClick.Invoke();
#endif
    }

    private void OnDisable()
    {
        if (selectRoutine != null) StopCoroutine(selectRoutine);
        selectRoutine = null;
        for (int i = 0; i < activeBlockedGroups.Count; i++)
        {
            CanvasGroup group = activeBlockedGroups[i];
            if (group == null || previousInteractable == null) continue;
            group.interactable = previousInteractable[i];
            group.blocksRaycasts = previousRaycasts[i];
        }
        activeBlockedGroups.Clear();
        if (EventSystem.current != null && previousSelection != null
            && previousSelection.activeInHierarchy)
            EventSystem.current.SetSelectedGameObject(previousSelection);
    }
}
