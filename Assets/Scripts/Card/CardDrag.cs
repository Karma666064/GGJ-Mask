using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rect;
    Canvas canvas;
    Vector2 startPos;

    public CardDropArea dropArea;
    public float returnSpeed = 12f;
    bool returning;
    private bool canPlay = false;

    int lastSiblingIndex;

    int indexUsed = 0;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        FightManager.enableDragDrop += UpdateCanPlay;
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

        rect.SetSiblingIndex(lastSiblingIndex);
    }

    public void UpdateCanPlay(bool bo)
    {
        canPlay = bo;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CardData tempoCard = rect.gameObject.GetComponent<CardData>();
        indexUsed = tempoCard.saveHand.IndexOf(tempoCard);
        PlayerState tempoPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        
        Debug.Log("PLAYER AP " + tempoPlayer.actionPoint);
        Debug.Log("TEMPO CARD COST " + tempoCard.cost);
        if (tempoPlayer.actionPoint < tempoCard.cost || !canPlay) {
            indexUsed = -1;
            dropArea.indexCardUsed = indexUsed;
            return;
        }

        startPos = rect.anchoredPosition;
        lastSiblingIndex = rect.GetSiblingIndex();
        
        

        Debug.Log("INDEX = " + indexUsed);
        dropArea.indexCardUsed = indexUsed;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (indexUsed == -1)
            return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
        if (indexUsed == -1)
            return; 
        if (eventData.pointerEnter != null)
        {
            CardDropArea dropArea = eventData.pointerEnter.GetComponentInParent<CardDropArea>();

            if (dropArea != null)
            {
                //Destroy(gameObject);
                returning = false;
                return;
            }
        }

        returning = true;
    }
}