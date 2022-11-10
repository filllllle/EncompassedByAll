using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WipeTaskPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField]
    Task ownerTask;

    [SerializeField]
    GameObject dirtPrefab;

    [SerializeField]
    RectTransform glassPanel;

    [SerializeField]
    Sprite glassPanelWithDetergent;

    GameObject towelObject = null;
    bool sprayedFlask = false;
    int dirtRemoved = 0;

    GameObject lastFirstSelectedGameObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show()
    {
        gameObject.SetActive(true);
        lastFirstSelectedGameObject = GameManager.Instance.EventSystem.firstSelectedGameObject;
        GameManager.Instance.EventSystem.firstSelectedGameObject = gameObject;

        for (int i = 0; i < 3; i++)
        {
            GameObject dirt = Instantiate(dirtPrefab, glassPanel);

            Rect dirtRect = dirt.GetComponent<RectTransform>().rect;

            Vector2 halfDirtWidth = new Vector2(dirtRect.width / 2, dirtRect.height / 2);
            Vector2 halfGlassPanelSize = new Vector2(glassPanel.rect.width / 2, glassPanel.rect.height / 2);

            float x = Random.Range(halfDirtWidth.x, glassPanel.rect.width - halfDirtWidth.x) - (halfGlassPanelSize.x);
            float y = Random.Range(halfDirtWidth.y, glassPanel.rect.height - halfDirtWidth.y) - (halfGlassPanelSize.y);
            dirt.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EventSystem.firstSelectedGameObject = lastFirstSelectedGameObject;
        sprayedFlask = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "flaska")
        {
            glassPanel.GetComponent<Image>().sprite = glassPanelWithDetergent;
            sprayedFlask = true;
        }

        if (eventData.pointerCurrentRaycast.gameObject.name == "handduk")
        {
            towelObject = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public Rect GetWorldRect(RectTransform rt, Vector2 scale)
    {
        // Convert the rectangle to world corners and grab the top left
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 topLeft = corners[0];

        // Rescale the size appropriately based on the current Canvas scale
        Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

        return new Rect(topLeft, scaledSize);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (towelObject)
        {
            towelObject = null;

            if (dirtRemoved == 3)
            {
                ownerTask.SetAsResolved();
                Hide();
            }
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (towelObject)
        {
            towelObject.transform.position = eventData.position;

            if (sprayedFlask)
            {
                foreach (GameObject item in eventData.hovered)
                {
                    if (item.CompareTag("Dirt"))
                    {
                        Destroy(item);
                        dirtRemoved++;
                    }
                }
                //Collider2D[] colliders = Physics2D.OverlapPointAll(eventData.position);
                //Debug.Log(colliders.Length);
                //foreach(Collider2D col in colliders)
                //{
                //    if(col.CompareTag("Dirt"))
                //    {
                //        Destroy(col.gameObject);
                //        dirtRemoved++;
                //    }
                //}
            }
        }
    }
}
