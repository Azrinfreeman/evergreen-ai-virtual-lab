using UnityEngine;

/// <summary>
/// Marks one learning stage as locally completed the moment its real finish route runs.
/// Attach this to the button that ends the lesson, experiment, or quiz, or to the object
/// that shows the final result. The id is set in the Inspector, for example "a6.eksperimen".
/// </summary>
public sealed class UiProgressMarker : MonoBehaviour
{
    [Tooltip("Stage id: a{1-7}.belajar, a{1-7}.eksperimen or a{1-7}.ujidiri.")]
    [SerializeField] private string progressId;

    private void OnEnable()
    {
        UiProgress.SetComplete(progressId);
    }
}
