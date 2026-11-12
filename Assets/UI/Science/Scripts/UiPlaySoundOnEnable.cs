using UnityEngine;

/// <summary>
/// Plays one clip when this object becomes active, for example a slot tick or a revealed
/// Seterusnya button. The clip is assigned in the Inspector. The settings switch silences
/// this source through UiSound; no new audio assets are required.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public sealed class UiPlaySoundOnEnable : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.55f;
    [SerializeField, Range(0.5f, 1.5f)] private float pitch = 1f;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
        source.spatialBlend = 0f;
    }

    private void OnEnable()
    {
        // Objects that are already active when the scene loads are not a learner action.
        if (clip == null || !UiSound.Enabled || Time.timeSinceLevelLoad < 0.5f) return;
        source.pitch = pitch;
        source.PlayOneShot(clip, volume);
    }
}
