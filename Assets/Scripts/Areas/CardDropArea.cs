using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardDrag card = eventData.pointerDrag.GetComponent<CardDrag>();
        if (card == null) return;

        Debug.Log(card.name + " déposée !");
    }
}