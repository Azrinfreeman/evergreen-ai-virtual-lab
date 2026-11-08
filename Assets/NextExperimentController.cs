using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextExperimentController : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private Transform nextParent;

    private void Start()
    {
        if (parent == null) parent = transform.parent;
        // Resolve from this page, not whichever sibling happens to be active.
        if (nextParent == null && parent != null && parent.parent != null)
        {
            int nextIndex = parent.GetSiblingIndex() + 1;
            if (nextIndex < parent.parent.childCount)
                nextParent = parent.parent.GetChild(nextIndex);
        }

        Button button = GetComponent<Button>();
        if (nextParent == null)
        {
            TextMeshProUGUI label = GetComponentInChildren<TextMeshProUGUI>();
            if (label != null) label.text = "Tamat";
            button.onClick.AddListener(tamat);
        }
        else button.onClick.AddListener(NextExperiment);
    }

    public void tamat()
    {
        SceneManager.LoadScene("Dashboard");
    }

    public void NextExperiment()
    {
        if (parent == null || nextParent == null) return;
        parent.gameObject.SetActive(false);
        nextParent.gameObject.SetActive(true);
    }
}
