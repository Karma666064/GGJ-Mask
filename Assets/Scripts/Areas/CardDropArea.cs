using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropArea : MonoBehaviour, IDropHandler
{
    public int indexCardUsed;
    public static Action<int> CardIsDropped;

    public void OnDrop(PointerEventData eventData)
    {
        CardDrag card = eventData.pointerDrag.GetComponent<CardDrag>();
        if (card == null) return;

        Debug.Log(card.name + " déposée !");
        if (indexCardUsed != -1)
            CardIsDropped?.Invoke(indexCardUsed);
    }
}