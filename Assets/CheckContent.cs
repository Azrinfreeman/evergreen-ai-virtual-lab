using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckContent : MonoBehaviour
{
    public List<Transform> item;

    public List<Transform> toAppear;

    public int l;

    public bool toDisappear;

    private bool completed;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (completed || item == null || item.Count == 0) return;

        l = 0;
        foreach (Transform requiredItem in item)
        {
            if (requiredItem == null || requiredItem.gameObject.activeSelf != toDisappear)
                return;
            l++;
        }

        // Activate once: polling must not reopen controls hidden by later actions.
        completed = true;
        foreach (Transform target in toAppear)
            if (target != null) target.gameObject.SetActive(true);
    }
}
