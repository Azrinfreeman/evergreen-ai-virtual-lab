using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public Transform tick;

    [Header("Akt6")]
    public string jenisAnodKatod;
    public Transform ConditionalCheck;

    public string nameScene;

    public Transform ButtonNext;

    // Start is called before the first frame update
    void Awake()
    {
        // Resolve early: physics triggers can fire before Start on a freshly enabled slot.
        tick = transform.GetChild(0);
    }

    // Update is called once `r frame
    void Update() { }

    public void OnDrop(PointerEventData eventData)
    {
        nameScene = SceneManager.GetActiveScene().name.ToString();
        Debug.Log("onDrop");
        if (!SceneManager.GetActiveScene().name.Equals("akt6"))
        {
            //Debug.Log("stuck");
            eventData.pointerDrag.transform.SetParent(transform);
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.transform.localPosition = new Vector3(0, 0, 0);

            tick.gameObject.SetActive(true);

            eventData.pointerDrag.transform.GetComponent<DragController>().enabled = false;

            if (SceneManager.GetActiveScene().name.Equals("akt5"))
            {
                eventData.pointerDrag.transform.localPosition = new Vector3(0, 100f, 0);
            }
        }
        else
        {
            Debug.Log("stuck");
        }
        GetComponent<Image>().enabled = false;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Generic drop slots in other experiments do not have electrode bindings.
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName != "akt6" && sceneName != "akt7") return;
        if (ConditionalCheck == null) return;

        if (SceneManager.GetActiveScene().name == "akt7")
        {
            if (ConditionalCheck.name == "3")
            {
                if (jenisAnodKatod == "anod")
                {
                    Debug.Log("anod");
                    if (collision.name.Equals("rubberfilm"))
                    {
                        collision.transform.GetComponent<DragController>().enabled = false;
                        //Debug.Log("stuck");
                        collision.transform.SetParent(transform);
                        //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                        collision.transform.localPosition = new Vector3(0, 0, 0);

                        tick.gameObject.SetActive(true);
                        ButtonNext.gameObject.SetActive(true);
                        //transform.gameObject.SetActive(false);
                    }
                }
            }
            if (ConditionalCheck.name == "2")
            {
                if (jenisAnodKatod == "anod")
                {
                    Debug.Log("anod");
                    if (collision.name.Equals("rubberfilm"))
                    {
                        collision.transform.GetComponent<DragController>().enabled = false;
                        //Debug.Log("stuck");
                        collision.transform.SetParent(transform);
                        //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                        collision.transform.localPosition = new Vector3(0, 0, 0);

                        tick.gameObject.SetActive(true);
                        ButtonNext.gameObject.SetActive(true);
                        //transform.gameObject.SetActive(false);
                    }
                }
            }
        }
        if (ConditionalCheck.name == "0 (2)")
        {
            if (jenisAnodKatod == "anod")
            {
                Debug.Log("anod");
                if (collision.name.Equals("anodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "anod2")
            {
                Debug.Log("anod");
                if (collision.name.Equals("anodElektrod2"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "katod")
            {
                Debug.Log("katod");
                if (collision.name.Equals("katodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "katod2")
            {
                Debug.Log("katod2");
                if (collision.name.Equals("katodElektrod2"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
        }
        else if (ConditionalCheck.name == "0 (3)")
        {
            if (jenisAnodKatod == "anod")
            {
                Debug.Log("anod");
                if (collision.name.Equals("anodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "katod")
            {
                Debug.Log("katod");
                if (collision.name.Equals("katodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
        }
        else if (ConditionalCheck.name == "0 (4)")
        {
            if (jenisAnodKatod == "anod")
            {
                Debug.Log("anod");
                if (collision.name.Equals("anodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "katod")
            {
                Debug.Log("katod");
                if (collision.name.Equals("katodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
        }
        else if (ConditionalCheck.name == "0 (8)")
        {
            if (jenisAnodKatod == "anod")
            {
                Debug.Log("anod");
                if (collision.name.Equals("anodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);
                    ButtonNext.gameObject.SetActive(true);
                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "katod")
            {
                Debug.Log("katod");
                if (collision.name.Equals("katodElektrod"))
                {
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);
                    ButtonNext.gameObject.SetActive(true);
                    //transform.gameObject.SetActive(false);
                }
            }
        }
        else if (ConditionalCheck.name == "2")
        {
            if (jenisAnodKatod == "anod")
            {
                if (collision.name.Equals("wayarAnod"))
                {
                    Debug.Log("anod2");
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            else if (jenisAnodKatod == "katod")
            {
                if (collision.name.Equals("watarKatod"))
                {
                    Debug.Log("katod2");
                    collision.transform.GetComponent<DragController>().enabled = false;
                    //Debug.Log("stuck");
                    collision.transform.SetParent(transform);
                    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
                    collision.transform.localPosition = new Vector3(0, 0, 0);

                    tick.gameObject.SetActive(true);

                    //transform.gameObject.SetActive(false);
                }
            }
            ConditionalCheck.GetComponent<ConditionalCheck>().CheckCondition();
        }
    }
}
