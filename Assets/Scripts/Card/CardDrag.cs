using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rect;
    Canvas canvas;
    Vector2 startPos;

    public float returnSpeed = 12f;
    bool returning;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (!returning) return;

        rect.anchoredPosition = Vector2.Lerp(
            rect.anchoredPosition,
            startPos,
            Time.deltaTime * returnSpeed
        );

        if (Vector2.Distance(rect.anchoredPosition, startPos) < 0.5f)
        {
            rect.anchoredPosition = startPos;
            returning = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rect.anchoredPosition;
        rect.SetAsLastSibling(); // passe au-dessus des autres cartes
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            CardDropArea dropArea = eventData.pointerEnter.GetComponentInParent<CardDropArea>();

            if (dropArea != null)
            {
                Destroy(gameObject);

                returning = true;
            }
        }

        returning = true;
    }
}