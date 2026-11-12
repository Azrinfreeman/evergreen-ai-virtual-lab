using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this on the "tick" child of an <c>ItemSlot</c>. While the tick is active the slot's
/// own Image (the drop-target cue) is hidden, so the cue does not show behind a placed item.
/// ItemSlot itself is not changed.
/// </summary>
public class UiHideSlotCueOnTick : MonoBehaviour
{
    private Image slotImage;

    private void OnEnable()
    {
        SetCueVisible(false);
    }

    private void OnDisable()
    {
        SetCueVisible(true);
    }

    private void SetCueVisible(bool visible)
    {
        if (slotImage == null && transform.parent != null)
            slotImage = transform.parent.GetComponent<Image>();
        if (slotImage != null)
            slotImage.enabled = visible;
    }
}
