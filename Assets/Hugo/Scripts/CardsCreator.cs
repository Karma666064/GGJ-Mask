using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardsCreator : MonoBehaviour
{
    public PlayerState player;

    public AllCardsSO allCards;
    public AllMaskSO allMasks;

    public List<Cards> deck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Cards> CreateDeck()
    {
        for (int i = 0; i < allCards.Cards.Length; i++)
        {
            for (int j = 0; j < 2; j++) {
                Cards newCard = gameObject.AddComponent<Cards>();

                newCard.card = allCards.Cards[i];
                newCard.allMask = allMasks;
                newCard.mask = player.playerMask;
                newCard.ChangeCardMask();
                deck.Add(newCard);
            }
        }
        
        return deck;
    }

    public void DEBUGPrintDeck()
    {
        foreach (var item in deck)
        {
            Debug.Log(item.currentName + "\n");
        }
    }

}