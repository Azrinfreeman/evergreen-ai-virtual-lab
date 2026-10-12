using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

public class CheckBehaviour : MonoBehaviour
{
    public static CheckBehaviour instance;

    void Awake()
    {
        instance = this;
    }

    public string BehaviourName;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
