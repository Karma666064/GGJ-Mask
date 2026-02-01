using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Rendering.UnifiedRayTracing;

public class PlayerState : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int shield;

    public int handStartSize = 5;
    public int handMaxSize = 7;
    public MaskState playerMask;
    public int actionPoint;
    public int actionPointMax;

    public List<CardData> hand;

    public List<CardData> decks;
    public List<CardData> discard;

    public List<Sprite> allMasksSprites;

    public Image uiMask;
    public Image uiMaskShadow;

    public CardsCreator cardsCreator;

    public GameObject HandZone;

    public TextMeshProUGUI hpBar;
    public TextMeshProUGUI shBar;
    public TextMeshProUGUI apBar;
    public Image hpJauge;

    public Sprite currentMaskSprite;

    public GameObject deckPool;

    public TextMeshProUGUI cardInDeck;

    public void Awake()
    {
        decks = cardsCreator.CreateDeck();
        StartCoroutine(DrawHand(handStartSize));
        PlayerChangeMask(playerMask);
        hpBar.text = "HP: " + maxHealth.ToString();
        shBar.text = "SH: " + shield.ToString();
    }

    public void PlayerGrowth()
    {
        maxHealth += 5;
        health = maxHealth;
        actionPointMax += 1;
        actionPointMax = Math.Clamp(actionPointMax, 0, 6);
        actionPoint = actionPointMax;
        UpdateUI();
    }

    public IEnumerator DrawHand(int numberCardDraw)
    {
        for (int i = 0; i < numberCardDraw; i++)
        {
            if (decks.Count != 0) {
                ChooseRandomCardsInDeck();
                cardInDeck.text = decks.Count.ToString() + "/18 CARDS";
                AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.draw);
                yield return new WaitForSeconds(0.5f);
            }
            else
                break;
        }
    }

    public void AddDiscardToDeck()
    {
        foreach (var item in discard)
        {
            decks.Add(item);
            item.transform.SetParent(deckPool.transform);
            Debug.Log("Here adding card");
        }
        discard.Clear();
    }

    public void AddHandToDeck()
    {
        foreach (var item in hand)
        {
            decks.Add(item);
            item.transform.SetParent(deckPool.transform);
            item.transform.position = item.transform.parent.position;
            Debug.Log("Here adding card");
        }
        hand.Clear();
    }

    public void ChooseRandomCardsInDeck()
    {
        int indexAleatoire = UnityEngine.Random.Range(0, decks.Count);

        if (hand.Count < 9) 
        {
            hand.Add(decks[indexAleatoire]);
            decks[indexAleatoire].saveHand = hand;
            decks[indexAleatoire].transform.SetParent(HandZone.transform);
        } else {
            discard.Add(decks[indexAleatoire]);
        }
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

        foreach (var item in decks)
        {
            item.mask = playerMask;
            item.ChangeCardMask();
        }

        foreach (var item in discard)
        {
            item.mask = playerMask;
            item.ChangeCardMask();
        }
        currentMaskSprite = allMasksSprites[(int)playerMask];
        uiMask.sprite = currentMaskSprite;
        uiMaskShadow.sprite = currentMaskSprite;
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

    public void TakeDamage(int damage)
    {
        Debug.Log("DAMAGE DONE" + damage);
        Debug.Log("HEATH" + health);

        int remainingDamage = damage - shield;
        remainingDamage = Math.Clamp(remainingDamage, 0, 999);

        shield -= damage;

        shield = Math.Clamp(shield, 0, 999);
        

        Debug.Log("REMAINING DAMAGE" + remainingDamage);

        health -= remainingDamage;
        UpdateUI();
        if (health <= 0)
            Die();

        health = Math.Clamp(health, 0, 999);
        AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.hurt);
    }

    public MaskState FindNewMask()
    {
        int rand = UnityEngine.Random.Range(0,2);

        if (playerMask == MaskState.angry)
        {
            if (rand == 0)
                return MaskState.joy;
            else
                return MaskState.sad;
        }

        if (playerMask == MaskState.sad)
        {
            if (rand == 0)
                return MaskState.joy;
            else
                return MaskState.angry;
        }

        if (playerMask == MaskState.joy)
        {
            if (rand == 0)
                return MaskState.sad;
            else
                return MaskState.angry;
        }
        
        return playerMask;
    }

    public void UpdateUI()
    {
        hpBar.text = "HP: " + health.ToString();
        shBar.text = "SP: " + shield.ToString();
        apBar.text = "AP: " + actionPoint.ToString() + "/" + actionPointMax.ToString();
        hpJauge.fillAmount = (float)health / (float)maxHealth;
    }

    public void DEBUGPrintEffectCard(CardData card)
    {
        Debug.Log($"Remaining Action point {actionPoint}!");
        Debug.Log($"Remaining Health point {health}!");
        Debug.Log($"Remaining Shield point {shield}!");
        Debug.Log($"Ennemy Suffer {card.damage} damage!");
        Debug.Log("\n");
    }

    public void  DEBUGPrintDiscard()
    {
        foreach (var item in discard)
        {
            Debug.Log("DISCARD : " + item.currentName);
        }
    }
}