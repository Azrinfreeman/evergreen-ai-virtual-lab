using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sound switch saved in PlayerPrefs. It mutes the game's sound-effect sources (the ones under
/// the scene root named "Sfx"), the background music (MusicLoopingController), and sources owned
/// by UiPlaySoundOnEnable, so other audio such as a future assistant voice is not silenced.
/// The setting is applied again whenever a scene finishes loading.
/// </summary>
public static class UiSound
{
    private const string SoundKey = "ui.sound";
    private const string EffectsRootName = "Sfx";

    public static bool Enabled
    {
        get => PlayerPrefs.GetInt(SoundKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
            PlayerPrefs.Save();
            Apply();
        }
    }

    public static void Apply()
    {
        bool enabled = Enabled;
        foreach (AudioSource source in Resources.FindObjectsOfTypeAll<AudioSource>())
        {
            if (source == null || !source.gameObject.scene.IsValid()) continue;
            bool isOwnedCue = source.GetComponent<UiPlaySoundOnEnable>() != null;
            if (!IsEffect(source.transform) && !IsMusic(source) && !isOwnedCue) continue;
            source.mute = !enabled;
        }
    }

    private static bool IsMusic(AudioSource source)
    {
        if (source.GetComponentInParent<MusicLoopingController>(true) != null) return true;
        MusicLoopingController music = MusicLoopingController.instance;
        return music != null && music.audioSources != null && music.audioSources.Contains(source);
    }

    private static bool IsEffect(Transform current)
    {
        while (current != null)
        {
            if (current.name == EffectsRootName) return true;
            current = current.parent;
        }
        return false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Load()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Apply();
    }
}
