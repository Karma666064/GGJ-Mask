using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int shield;

    public int handStartSize = 5;
    public int handMaxSize = 7;
    public MaskState playerMask;
    public int actionPoint;

    public List<Cards> hand;

    public List<Cards> decks;

    private CardsCreator cardsCreator;

    public void Awake()
    {
        cardsCreator = GetComponent<CardsCreator>();
        decks = cardsCreator.CreateDeck();
        cardsCreator.DEBUGPrintDeck(decks);
        DrawHand();
        cardsCreator.DEBUGPrintDeck(decks);
        PlayerChangeMask(playerMask);
    }

    public void DrawHand()
    {
        for (int i = 0; i < handStartSize; i++)
        {
            if (decks.Count != 0)
                ChooseRandomCardsInDeck();
            else
                return;
        }
    }

    public void  ChooseRandomCardsInDeck()
    {
        int indexAleatoire = UnityEngine.Random.Range(0, decks.Count);
        hand.Add(decks[indexAleatoire]);
        decks.Remove(decks[indexAleatoire]);
    }

    public void PlayerChangeMask(MaskState _mask)
    {
        playerMask = _mask;
        foreach (var item in hand)
        {
            item.mask = playerMask;
            item.ChangeCardMask();
        }
    }

    public void DEBUGPrintHand()
    {
        foreach (var item in hand)
        {
            item.DEBUGPrintName();
        }
    }

    public void Die()
    {
        Debug.Log("Player is DEAD");
    }


    public void DEBUGPrintEffectCard(Cards card)
    {
        Debug.Log($"Remaining Action point {actionPoint}!");
        Debug.Log($"Remaining Health point {health}!");
        Debug.Log($"Remaining Shield point {shield}!");
        Debug.Log($"Ennemy Suffer {card.damage} damage!");
        Debug.Log("\n");
    }
}