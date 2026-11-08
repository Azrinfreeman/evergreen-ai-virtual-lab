using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Adds a persistent outline for keyboard selection, separate from hover tint.</summary>
[RequireComponent(typeof(Outline))]
public sealed class UiFocusIndicator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Color focusColor = new Color32(11, 11, 59, 255);
    [SerializeField, Min(1f)] private float outlineWidth = 4f;
    [SerializeField] private Color borderColor = new Color32(70, 85, 107, 255);
    [SerializeField, Min(0f)] private float borderWidth = 1.5f;
    private Outline focusOutline;
    private bool selected;

    private void Awake()
    {
        focusOutline = GetComponent<Outline>();
        focusOutline.useGraphicAlpha = false;
        ApplyOutline();
    }

    private void OnEnable()
    {
        selected = EventSystem.current != null
            && EventSystem.current.currentSelectedGameObject == gameObject;
        ApplyOutline();
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
        ApplyOutline();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
        ApplyOutline();
    }

    private void ApplyOutline()
    {
        if (focusOutline == null) return;
        float width = selected ? outlineWidth : borderWidth;
        focusOutline.effectColor = selected ? focusColor : borderColor;
        focusOutline.effectDistance = new Vector2(width, -width);
        focusOutline.enabled = width > 0f;
    }

    private void OnDisable()
    {
        if (focusOutline != null) focusOutline.enabled = false;
    }
}
