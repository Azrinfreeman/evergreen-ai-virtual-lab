using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void DisableObject()
    {
        transform.gameObject.SetActive(false);
    }
}
