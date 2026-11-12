using UnityEngine;

/// <summary>Single switch for non-essential UI motion. The settings toggle (planned) writes this key.</summary>
public static class UiMotion
{
    private const string ReduceMotionKey = "ui.reduceMotion";

    public static bool Reduced
    {
        get => PlayerPrefs.GetInt(ReduceMotionKey, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(ReduceMotionKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
