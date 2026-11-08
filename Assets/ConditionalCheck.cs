using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionalCheck : MonoBehaviour
{
    public int procedurNum;

    [Tooltip("Type of fading")]
    public Condi condition;

    private void Awake() { }

    [SerializeField]
    private List<Transform> conditionToMeet;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Transform correctButton;

    public enum Condi
    {
        Number,
        Percent,
        Text,

        Transform,
    }

    // Start is called before the first frame update
    void Start()
    {
        // Duplicate procedure pages retain Unity's suffix, e.g. "5 (1)".
        // Preserve an explicit Inspector value for pages without a numeric name.
        string number = transform.name.Split(' ')[0];
        if (Int32.TryParse(number, out int parsedNumber))
            procedurNum = parsedNumber;
    }

    public void CheckCondition()
    {
        switch (condition)
        {
            //This statement works
            case Condi.Number: //Only this piece of code works
                break;

            //This statement doesn't work
            case Condi.Percent:

                //Debug.Log("continue");
                nextButton.GetComponent<Transform>().gameObject.SetActive(true);
                break;

            //This statement doesn't work
            case Condi.Text:
                break;

            //This statement doesn't work
            case Condi.Transform:
                bool isContinue = true;
                for (int i = 0; i < conditionToMeet.Count; i++)
                {
                    if (!conditionToMeet[i].gameObject.activeSelf)
                    {
                        Debug.Log("stop");
                        isContinue = false;
                        break;
                    }
                }

                if (isContinue)
                {
                    Debug.Log(isContinue);
                    nextButton.GetComponent<Transform>().gameObject.SetActive(true);
                }
                break;
        }
        correctButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update() { }
}
