using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
using UnityEngine.UI;

public class AssignBehaviour : MonoBehaviour
{
    public string AssignToBehaviour;
    public ChatGPT chatGpt;

    // Start is called before the first frame update
    void Start()
    {
        chatGpt = GameObject.Find("ChatGPT").GetComponent<ChatGPT>();
        GetComponent<Button>().onClick.AddListener(SetBehaviours);
    }

    public void SetBehaviours()
    {
        GameObject.Find("CurrentBehaviour").transform.GetComponent<CheckBehaviour>().BehaviourName =
            AssignToBehaviour;
        ;
        chatGpt.ChangePrompt();
    }

    // Update is called once per frame
    void Update() { }
}
