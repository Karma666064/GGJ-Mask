using System;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public EnemySO statSO;
    public int health;
    public int maxHealth;
    public int shield;
    public int damage;
    public new string name;
    public int healPower;
    public int shieldPower;
    public MaskState mask;
    public MaskState nextMask;
    public EnemyAttack[] pattern;
    public EnemyAttack previewAttack;

    public bool hasSpecial;

    private int countAttack = 0;
    private int rand = 0;
    void Awake()
    {
        InitStat();
    }

    public void InitStat()
    {
        health = statSO.health;
        shield = statSO.shield;
        damage = statSO.damage;
        name = statSO.name;
        pattern = statSO.pattern;
        mask = statSO.mask;
        hasSpecial = statSO.hasSpecial;
        healPower = statSO.healPower;
        shieldPower = statSO.shieldPower;
        maxHealth = statSO.maxHealth;
    }

    public void ChoosePreviewAttack()
    {
        rand = UnityEngine.Random.Range(0, 2);
        if (countAttack != 3) {
            Debug.Log(UnityEngine.Random.Range(0, pattern.Length));
            previewAttack = pattern[UnityEngine.Random.Range(0, pattern.Length)];
            if (hasSpecial)
                countAttack++;
        } else
        {
            previewAttack = EnemyAttack.Special;
            countAttack = 0;
        }

        if (previewAttack == EnemyAttack.StateChange) {
            PreviewNextMask();
            Debug.Log("The next mask : " + nextMask + " !");
        }
        Debug.Log("Preview Attack is : " + previewAttack);
    }
    public void Heal()
    {
        health += healPower;
        health = Math.Clamp(health, -1, maxHealth);
    }

    public void PreviewNextMask()
    {
       if (nextMask == MaskState.angry)
        {
            if (rand == 0)
                nextMask = MaskState.joy;
            else 
                nextMask = MaskState.sad;
            return;
        }
        if (nextMask == MaskState.joy)
        {
            if (rand == 0)
                nextMask = MaskState.angry;
            else 
                nextMask = MaskState.sad;
            return;
        }
        if (nextMask == MaskState.sad)
        {
            if (rand == 0)
                nextMask = MaskState.angry;
            else 
                nextMask = MaskState.joy;
            return;
        }
    }

    public void ChangeState()
    {
       mask = nextMask;
    }


    public void Shield()
    {
        shield += shieldPower;
    }

    public void Special()
    {
        Debug.Log("It's a special attack!");
    }


    public void Die()
    {
        Debug.Log("Enemy Died");
    }

    public void DEBUGInfoEnnemy()
    {
        Debug.Log($"Enemy : {name}!");
        Debug.Log($"Current Health : {health}!");
        Debug.Log($"Current Shield : {shield}!");
        Debug.Log($"Current Mask : {mask}!");
        Debug.Log($"Current Power : {damage}!");
    }
}
