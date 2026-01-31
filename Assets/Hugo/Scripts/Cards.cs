using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    public CardSO card;
    public AllMaskSO allMask;

    [HideInInspector] public CardType type;
    [HideInInspector] public int cost;
    [HideInInspector] public string currentDescription;
    [HideInInspector] public MaskState mask;
    [HideInInspector] public GameObject currentSprite;
    [HideInInspector] public string currentName;
    [HideInInspector] public int damage;
    [HideInInspector] public int heal;
    [HideInInspector] public int shield;
    [HideInInspector] public CardEffect effect;

    public void ChangeCardMask()
    {
        type = card.type;
        cost = card.cost;
        switch (mask)
        {
            case MaskState.angry:
                currentDescription = card.allDescriptions[(int)MaskState.angry];
                ApplyMask(allMask.allMasks[(int)MaskState.angry]);
                break;
            case MaskState.joy:
                currentDescription = card.allDescriptions[(int)MaskState.joy];
                ApplyMask(allMask.allMasks[(int)MaskState.joy]);
                break;
            case MaskState.sad:
                currentDescription = card.allDescriptions[(int)MaskState.sad];
                ApplyMask(allMask.allMasks[(int)MaskState.sad]);
                break;
            default:
                break;
        }
    }

    public void ApplyMask(MaskSO mask)
    {
        int lifeSteal = 0;

        if (mask.maskState == MaskState.joy && type == CardType.attack) {
            lifeSteal = card.damage / 2;
            lifeSteal = Math.Max(lifeSteal, 1);
        }
    
        damage = card.damage * mask.damageMultipler;
        heal = card.heal * mask.healMultiplier + lifeSteal;
        shield = card.shield * mask.shieldMultiplier;
        currentName = card.allNames[(int)mask.maskState];

        if (card.currentEffect != CardEffect.none)
            ChangeEffect(mask.maskState);
        else
            effect = card.currentEffect;
    }

    public void ChangeEffect(MaskState _state)
    {
        switch (_state)
        {
            case MaskState.angry:
                effect = card.allEffects[(int)MaskState.angry];
                break;
            case MaskState.joy:
                effect = card.allEffects[(int)MaskState.joy];
                break;
            case MaskState.sad:
                effect = card.allEffects[(int)MaskState.sad];
                break;
            default:
                break;
        }
    
    }

    public void DEBUGAllPrint()
    {
        Debug.Log("Name = " + currentName);
        Debug.Log("Type = " + type);
        Debug.Log("Damage = " + damage);
        Debug.Log("Heal = " + heal);
        Debug.Log("Shield = " + shield);
        Debug.Log("Effect = " + effect);
        Debug.Log("Cost = " + cost);
        Debug.Log("Description = " + currentDescription);
    }

    public void DEBUGPrintName()
    {
        Debug.Log("Name = " + currentName);
    }
}
