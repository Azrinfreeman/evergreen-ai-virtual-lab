using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Settings dialog with reduce-motion and sound switches. Both values are saved in PlayerPrefs,
/// so they survive scene changes and restarts. Toggle labels are text, never colour alone.
/// </summary>
public sealed class UiSettingsPrompt : MonoBehaviour
{
    [Header("Switches")]
    [SerializeField] private Button reduceMotionButton;
    [SerializeField] private TMP_Text reduceMotionLabel;
    [SerializeField] private Button soundButton;
    [SerializeField] private TMP_Text soundLabel;

    [Header("Open and close")]
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Color onColor = new Color32(22, 101, 52, 255);
    [SerializeField] private Color offColor = new Color32(70, 85, 107, 255);

    private void Awake()
    {
        if (openButton != null) openButton.onClick.AddListener(Open);
        if (reduceMotionButton != null) reduceMotionButton.onClick.AddListener(ToggleReduceMotion);
        if (soundButton != null) soundButton.onClick.AddListener(ToggleSound);
        if (closeButton != null) closeButton.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Open()
    {
        if (promptRoot == null) return;
        Refresh();
        promptRoot.SetActive(true);
    }

    public void Close()
    {
        if (promptRoot == null) return;
        promptRoot.SetActive(false);
    }

    public void ToggleReduceMotion()
    {
        UiMotion.Reduced = !UiMotion.Reduced;
        Refresh();
    }

    public void ToggleSound()
    {
        UiSound.Enabled = !UiSound.Enabled;
        Refresh();
    }

    private void Refresh()
    {
        SetSwitch(reduceMotionLabel, UiMotion.Reduced);
        SetSwitch(soundLabel, UiSound.Enabled);
    }

    private void SetSwitch(TMP_Text label, bool on)
    {
        if (label == null) return;
        label.text = on ? "Hidup" : "Mati";
        label.color = on ? onColor : offColor;
    }
}
