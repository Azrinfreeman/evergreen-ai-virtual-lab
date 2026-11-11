using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DragController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private string objectName;

    [SerializeField]
    private Canvas canvas;

    [Header("Return on invalid drop")]
    [Tooltip("When released outside a drop slot, glide back to the position where the drag started. Pilot scenes only.")]
    [SerializeField]
    private bool returnToOrigin;

    [SerializeField, Range(0.05f, 0.5f)]
    private float returnDuration = 0.18f;

    private RectTransform rectTransform;
    private Vector2 dragStartPosition;
    private Transform dragStartParent;
    private Coroutine returnRoutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (returnToOrigin && rectTransform != null)
        {
            FinishReturn();
            dragStartPosition = rectTransform.anchoredPosition;
            dragStartParent = rectTransform.parent;
        }

        if (objectName == null)
        {
            //  Debug.Log("OnBeginDrag");
            GetComponent<CanvasGroup>().alpha = .6f;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            //        GameObject.Find("cartoon").GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (objectName.Equals("Dawai"))
        {
            //  Debug.Log("OnBeginDrag");
            GetComponent<CanvasGroup>().alpha = 1f;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //        GameObject.Find("cartoon").GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (objectName.Equals("katod"))
        {
            //  Debug.Log("OnBeginDrag");
            GetComponent<CanvasGroup>().alpha = 1f;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //        GameObject.Find("cartoon").GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (objectName.Equals("getahasli"))
        {
            Debug.Log("drag");
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -32f);
            ;
            //  Debug.Log("OnBeginDrag");
            GetComponent<CanvasGroup>().alpha = .8f;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            //        GameObject.Find("cartoon").GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //   Debug.Log("OnEndDrag");
        if (SceneManager.GetActiveScene().name != "akt4")
        {
            GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        }
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        GetComponent<BoxCollider2D>().enabled = true;

        if (returnToOrigin && rectTransform != null && rectTransform.parent == dragStartParent)
        {
            if (returnRoutine != null) StopCoroutine(returnRoutine);
            returnRoutine = StartCoroutine(ReturnToStart());
        }
    }

    // True while the item is still a free, unplaced draggable.
    private bool StillUnplaced()
    {
        return this != null && enabled && rectTransform != null && rectTransform.parent == dragStartParent;
    }

    private void FinishReturn()
    {
        if (returnRoutine == null) return;
        StopCoroutine(returnRoutine);
        returnRoutine = null;
        rectTransform.anchoredPosition = dragStartPosition;
    }

    private IEnumerator ReturnToStart()
    {
        // Trigger-based slots (akt6, akt7) only see the item after the collider is re-enabled,
        // on the next physics steps. Give them two steps to place it before deciding to return.
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        if (!StillUnplaced())
        {
            returnRoutine = null;
            yield break;
        }

        if (UiMotion.Reduced)
        {
            rectTransform.anchoredPosition = dragStartPosition;
            returnRoutine = null;
            yield break;
        }

        Vector2 from = rectTransform.anchoredPosition;
        for (float t = 0f; t < returnDuration; t += Time.unscaledDeltaTime)
        {
            if (!StillUnplaced())
            {
                returnRoutine = null;
                yield break;
            }
            float k = Mathf.Clamp01(t / returnDuration);
            float eased = 1f - (1f - k) * (1f - k);
            rectTransform.anchoredPosition = Vector2.LerpUnclamped(from, dragStartPosition, eased);
            yield return null;
        }
        rectTransform.anchoredPosition = dragStartPosition;
        returnRoutine = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update() { }
}
