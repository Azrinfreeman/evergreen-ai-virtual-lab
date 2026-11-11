using UnityEngine;

/// <summary>
/// Local completion progress for the Dashboard activity hubs, saved in PlayerPrefs.
/// Each stage has an explicit id of the form "a{activity}.{stage}", for example "a6.eksperimen".
/// Stages: belajar (lesson), eksperimen, ujidiri. Markers and badges carry the id in the Inspector,
/// so it does not depend on which scene is active or on object names.
/// </summary>
public static class UiProgress
{
    private const string KeyPrefix = "ui.progress.";
    private const int ActivityCount = 7;
    private static readonly string[] Stages = { "belajar", "eksperimen", "ujidiri" };

    public static bool IsComplete(string id)
    {
        return !string.IsNullOrEmpty(id) && PlayerPrefs.GetInt(KeyPrefix + id, 0) == 1;
    }

    public static void SetComplete(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        PlayerPrefs.SetInt(KeyPrefix + id, 1);
        PlayerPrefs.Save();
    }

    /// <summary>Clears every stage of every activity. Called by "Set semula kemajuan".</summary>
    public static void ResetAll()
    {
        for (int activity = 1; activity <= ActivityCount; activity++)
            foreach (string stage in Stages)
                PlayerPrefs.DeleteKey(KeyPrefix + "a" + activity + "." + stage);

        // Keys written by the first version (scene name + stage number) are removed too.
        string[] legacyScenes =
        {
            "Dashboard", "akt3", "akt4", "akt5", "akt6", "akt7",
            "Uji Diri 1", "Uji Diri 2", "Uji Diri 3", "Uji Diri 4", "Uji Diri 5", "Uji Diri 6", "Uji Diri 7"
        };
        foreach (string scene in legacyScenes)
            for (int stage = 0; stage <= 2; stage++)
                PlayerPrefs.DeleteKey(KeyPrefix + scene + ".stage" + stage);

        PlayerPrefs.Save();
    }
}
