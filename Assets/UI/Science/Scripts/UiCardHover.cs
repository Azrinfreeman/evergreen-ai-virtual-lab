using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>Darkens a card's border while the pointer is over it or it has keyboard focus.</summary>
[RequireComponent(typeof(Graphic))]
public sealed class UiCardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Color restColor = new Color32(0x46, 0x55, 0x6B, 0xFF);
    [SerializeField] private Color activeColor = new Color32(0x0B, 0x0B, 0x3B, 0xFF);
    [SerializeField, Min(0f)] private float duration = 0.12f;

    private Graphic border;
    private Coroutine fade;
    private bool pointerOver;
    private bool focused;

    private void Awake() => border = GetComponent<Graphic>();

    private void OnEnable() => Apply(restColor, true);

    public void OnPointerEnter(PointerEventData eventData) { pointerOver = true; Refresh(); }
    public void OnPointerExit(PointerEventData eventData) { pointerOver = false; Refresh(); }
    public void OnSelect(BaseEventData eventData) { focused = true; Refresh(); }
    public void OnDeselect(BaseEventData eventData) { focused = false; Refresh(); }

    private void Refresh() => Apply(pointerOver || focused ? activeColor : restColor, false);

    private void Apply(Color target, bool instant)
    {
        if (fade != null) StopCoroutine(fade);
        fade = null;
        if (instant || UiMotion.Reduced || duration <= 0f || !isActiveAndEnabled)
        {
            border.color = target;
            return;
        }
        fade = StartCoroutine(Fade(target));
    }

    private IEnumerator Fade(Color target)
    {
        Color from = border.color;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            border.color = Color.Lerp(from, target, t / duration);
            yield return null;
        }
        border.color = target;
        fade = null;
    }
}
