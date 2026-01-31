using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class FightManager : MonoBehaviour
{
    public PlayerState player;
    public EnemyState enemy;
    
    private List<Cards> deck;
    public List<Cards> hand;
    private int finalDamage;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerState>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<EnemyState>();
        deck = player.decks;
        hand = player.hand;
        //UseCard(0);
        EnemyPreviewAction();
        EnemyAction();


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
                player.health -= finalDamage;
                if (player.health <= 0)
                    player.Die();
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

        enemy.DEBUGInfoEnnemy();
    }


    public void UseCard(int numberCardSelected)
    {
        if (player.actionPoint - hand[numberCardSelected].cost < 0) {
            Debug.Log("Can't Use This Card, not Enough AP!");
            return;
        }
        else
            player.actionPoint -= hand[numberCardSelected].cost;
        
        player.health += hand[numberCardSelected].heal;
        player.health = Math.Clamp(player.health, 0, player.maxHealth);
        player.shield += hand[numberCardSelected].shield;

        finalDamage = CalculateFinalDamage(hand[numberCardSelected].damage, player.playerMask, enemy.mask);
        enemy.health -= finalDamage;

        if (enemy.health <= 0)
            enemy.Die();
        
        hand[numberCardSelected].DEBUGAllPrint();
        Debug.Log("\n");
        DEBUGPrintEffectCard(hand[numberCardSelected]);

        hand.Remove(hand[numberCardSelected]);
        Debug.Log("Number Cards in Hand:" + hand.Count);
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

    public void DEBUGPrintEffectCard(Cards card)
    {
        Debug.Log($"Card Name = {card.currentName}!");
        Debug.Log($"Remaining Action point {player.actionPoint}!");
        Debug.Log($"Remaining Health point {player.health}!");
        Debug.Log($"Remaining Shield point {player.shield}!");
        Debug.Log($"{enemy.name} Suffers {finalDamage} damage! and has {enemy.health} remaining!");
        Debug.Log("\n");
    }
}
