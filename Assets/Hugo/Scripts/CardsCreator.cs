using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsCreator : MonoBehaviour
{
    public PlayerState player;

    public AllCardsSO allCards;
    public AllMaskSO allMasks;

    public CardDropArea dropArea;

    public List<CardData> deck;
    public GameObject physicalCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<CardData> CreateDeck()
    {
        for (int i = 0; i < allCards.Cards.Length; i++)
        {
            for (int j = 0; j < 2; j++) {

                GameObject wholecard = Instantiate(physicalCard, transform.position, Quaternion.identity, transform);

                wholecard.GetComponent<CardDrag>().dropArea = dropArea;
                
                CardData newCardData = wholecard.AddComponent<CardData>();

                newCardData.card = allCards.Cards[i];
                newCardData.allMask = allMasks;
                newCardData.mask = player.playerMask;
                newCardData.ChangeCardMask();
                deck.Add(newCardData);
            }
        }
        return deck;
    }

    public void DEBUGPrintDeck(List<CardData> _deck)
    {
        Debug.Log(deck.Count);
        foreach (var item in _deck)
        {
            Debug.Log(item.currentName + "\n");
        }
    }

}