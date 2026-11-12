using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Slight scale-up while an object is dragged. Works next to DragController (which is left unchanged)
/// and only reacts while that DragController is enabled. Scale only; skipped when UiMotion.Reduced.
/// </summary>
[RequireComponent(typeof(DragController))]
public sealed class UiDragLift : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField, Range(1f, 1.3f)] private float liftScale = 1.08f;

    private Vector3 restScale;
    private bool lifted;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var drag = GetComponent<DragController>();
        if (drag == null || !drag.enabled || UiMotion.Reduced) return;
        restScale = transform.localScale;
        transform.localScale = restScale * liftScale;
        lifted = true;
    }

    public void OnEndDrag(PointerEventData eventData) => Restore();

    private void OnDisable() => Restore();

    private void Restore()
    {
        if (!lifted) return;
        transform.localScale = restScale;
        lifted = false;
    }
}
