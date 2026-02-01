using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class FightManager : MonoBehaviour
{
    public PlayerState player;
    public EnemyState enemy;
    
    private List<CardData> deck;
    public List<CardData> hand;
    private int finalDamage;
    public Turn activeTurn;
    public static Action<bool> enableDragDrop;

    private bool hasPlayCard = false;

    private int turnCount = 3;

    public UnityEngine.UI.Button renewButton;

    public TextMeshProUGUI MaskTimer;

    public Sprite iceDebuff;
    public Sprite fireDebuff;
    public Sprite noneDebuff;

    public UnityEngine.UI.Image debuffImage;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyState>();
        //deck = player.decks;
        //hand = player.hand;

        EnemyPreviewAction();
        //EnemyAction();
        CardDropArea.CardIsDropped += UseCard;
        EnemyState.NextLevel += ChangeLevel;

        StartCoroutine(WaitHand());
    }

    IEnumerator WaitHand()
    {
        yield return new WaitForSeconds(4f);
        enableDragDrop?.Invoke(true);

    }

    public void ChangeLevel()
    {
        StartCoroutine(WaitTransition()); 
    }

    public IEnumerator WaitTransition()
    {
        yield return StartCoroutine(AudioManager.Instance.AnimeTransition());

        debuffImage.sprite = noneDebuff;
        ForceRenewDeck();
        player.PlayerGrowth();

        turnCount = 3;
        MaskTimer.text = "Turn Left: " + turnCount.ToString();
        enemy.ChangeSO();
        enableDragDrop?.Invoke(true);
        activeTurn = Turn.player;
    }

    public void ChangeColorRenew(Color c)
    {
        ColorBlock cb = renewButton.colors;
        cb.normalColor = c;
        cb.highlightedColor = c;
        cb.pressedColor = c;
        cb.selectedColor = c;

        renewButton.colors = cb;

    }

    public void EnemyPreviewAction()
    {
        enemy.ChoosePreviewAttack();
    }

    public void EnemyAction()
    {
        switch (enemy.previewAttack)
        {
            case EnemyAttack.Attack:
                finalDamage = CalculateFinalDamage(enemy.damage, enemy.mask, player.playerMask);
                player.TakeDamage(finalDamage);
                break;
            case EnemyAttack.Heal:
                enemy.Heal();
                break;
            case EnemyAttack.Shield:
                enemy.Shield();
                break;
            case EnemyAttack.StateChange:
                enemy.ChangeState();
                break;
            case EnemyAttack.Special:
                enemy.Special();
                break;
            default:
                break;
        }

        //enemy.DEBUGInfoEnnemy();
    }


    public void UseCard(int numberCardDataSelected)
    {
        if (activeTurn == Turn.player) {
            if (player.actionPoint - player.hand[numberCardDataSelected].cost < 0) {
                Debug.Log("Can't Use This Card, not Enough AP!");
                return;
            }
            else
                player.actionPoint -= player.hand[numberCardDataSelected].cost;

            if (player.hand[numberCardDataSelected].heal != 0)
                AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.heal);
            
            if (player.hand[numberCardDataSelected].shield != 0)
                AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.shield);

            player.health += player.hand[numberCardDataSelected].heal;
            player.health = Math.Clamp(player.health, 0, player.maxHealth);
            
            player.shield += player.hand[numberCardDataSelected].shield;
            
            
            
            finalDamage = CalculateFinalDamage(player.hand[numberCardDataSelected].damage, player.playerMask, enemy.mask);
            

            if (player.hand[numberCardDataSelected].type == CardType.attack) 
            {
                if (player.playerMask == MaskState.angry)
                    enemy.debuff = NegativeEffect.burn;
                if (player.playerMask == MaskState.sad)
                    enemy.debuff = NegativeEffect.freeze;
            }

            ApplyDebuffMaskEffect(player.hand[numberCardDataSelected]); 
       
            player.hand[numberCardDataSelected].transform.SetParent(player.deckPool.transform);
            player.hand[numberCardDataSelected].transform.position = player.deckPool.transform.position;
            player.discard.Add(player.hand[numberCardDataSelected]);
            player.hand.Remove(player.hand[numberCardDataSelected]);
            player.DEBUGPrintDiscard();
            hasPlayCard = true;
            ChangeColorRenew(Color.gray);
            enemy.TakeDamage(finalDamage);
            player.UpdateUI();
        }
    }

    public void EndTurn()
    {
        if (activeTurn == Turn.player) {
            enableDragDrop?.Invoke(false);
            activeTurn = Turn.enemy;
            StartCoroutine(EnemyTurn());
        }
    }

    public void ForceRenewDeck()
    {
        enableDragDrop?.Invoke(false);
        activeTurn = Turn.enemy;
        StartCoroutine(ForceRenew());
    }

    public IEnumerator ForceRenew()
    {
        player.AddDiscardToDeck();
        player.AddHandToDeck();
        yield return StartCoroutine(player.DrawHand(player.handStartSize)); 
    }

    public void RenewDeck()
    {
        if (activeTurn == Turn.player && hasPlayCard == false) {
            enableDragDrop?.Invoke(false);
            activeTurn = Turn.enemy;
            StartCoroutine(Renew());
        }
    }

    public IEnumerator Renew()
    {
        player.AddDiscardToDeck();
        player.AddHandToDeck();
        yield return StartCoroutine(player.DrawHand(player.handStartSize - 2));
        yield return StartCoroutine(EnemyTurn());
    }

    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        EnemyAction();
        Debug.Log("Enemy Action!!");
        yield return new WaitForSeconds(1f);
        enemy.ChoosePreviewAttack();
        yield return StartCoroutine(player.DrawHand(2));
        activeTurn = Turn.player;
        hasPlayCard = false;
        enableDragDrop?.Invoke(true);
        ChangeColorRenew(Color.white);
        player.actionPoint = player.actionPointMax;
        player.UpdateUI();
        TriggerRandomMaskChange();

       
        if (enemy.debuff == NegativeEffect.burn)
        {
            enemy.TakeDamage(2);
        }

        enemy.debuffRemainingTurn--;
        if (enemy.debuffRemainingTurn <= 0) {
            enemy.debuff = NegativeEffect.none;
            debuffImage.sprite = noneDebuff;
        }
    }

    public void TriggerRandomMaskChange()
    {
        if (turnCount <= 1)
        {
            player.PlayerChangeMask(player.FindNewMask());
            turnCount = 3;
            MaskTimer.text = "Turn Left: " + turnCount.ToString();
            return;
        }
        turnCount--;
        MaskTimer.text = "Turn Left: " + turnCount.ToString();
    }

    public void ApplyDebuffMaskEffect(CardData card)
    {
       if (card.type == CardType.attack) 
        {
            if (player.playerMask == MaskState.angry) {
                enemy.debuff = NegativeEffect.burn;
                debuffImage.sprite = fireDebuff;
            }
            if (player.playerMask == MaskState.sad) {
                enemy.debuff = NegativeEffect.freeze;
                debuffImage.sprite = iceDebuff;
            }
            enemy.debuffRemainingTurn = 2;
        } 
    }

    public int CalculateFinalDamage(int damage, MaskState maskAttack, MaskState maskDefense)
    {
        if (damage == 0)
            return 0;
        
        if (maskAttack == MaskState.angry)
        {
            switch (maskDefense)
            {
                case MaskState.angry:
                    return damage;
                case MaskState.joy:
                    return damage * 2;
                case MaskState.sad:
                    return damage / 2;
                default:
                    return damage;
            }
        }
        if (maskAttack == MaskState.sad)
        {
            switch (maskDefense)
            {
                case MaskState.angry:
                    return damage * 2;
                case MaskState.joy:
                    return damage / 2;
                case MaskState.sad:
                    return damage;
                default:
                    return damage;
            }
        }
        if (maskAttack == MaskState.joy)
        {
            switch (maskDefense)
            {
                case MaskState.angry:
                    return damage / 2;
                case MaskState.joy:
                    return damage;
                case MaskState.sad:
                    return damage * 2;
                default:
                    return damage;
            }
        }
        return damage;
    }

    public void DEBUGPrintEffectCard(CardData card)
    {
        Debug.Log($"Card Name = {card.currentName}!");
        Debug.Log($"Remaining Action point {player.actionPoint}!");
        Debug.Log($"Remaining Health point {player.health}!");
        Debug.Log($"Remaining Shield point {player.shield}!");
        Debug.Log($"{enemy.name} Suffers {finalDamage} damage! and has {enemy.health} remaining!");
        Debug.Log("\n");
    }
}
