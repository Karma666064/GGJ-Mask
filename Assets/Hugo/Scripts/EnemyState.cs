using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyState : MonoBehaviour
{
    public EnemySO[] statSO;
    private EnemySO currentSO;

    public static Action NextLevel;

    private int level = 0;
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
    public NegativeEffect debuff = NegativeEffect.none;

    public int debuffRemainingTurn = 0;

    public bool hasSpecial;

    private int countAttack = 0;
    private int rand = 0;

    [HideInInspector] public Sprite currentMaskSprite;
    public Sprite[] allMasksSprites;

    public Image uiMask;
    public Image uiMaskShadow;

    public TextMeshProUGUI hpBar;
    public TextMeshProUGUI shBar;
    public Image hpJauge;

    public Sprite attackPreview;
    public Sprite shieldPreview;
    public Sprite healPreview;
    public Image previewImage;

    void Awake()
    {
        ChangeSO();
    }

    public void ChangeSO()
    {
        if (level < statSO.Length) {
            currentSO = statSO[level];
            InitStat();
            UpdateUI();
        } else
        {
            Debug.Log("YOU WIINN");
            SceneManager.LoadScene("Victory");
        }
    }

    public void InitStat()
    {
        health = currentSO.health;
        shield = currentSO.shield;
        damage = currentSO.damage;
        name = currentSO.name;
        pattern = currentSO.pattern;
        mask = currentSO.mask;
        hasSpecial = currentSO.hasSpecial;
        healPower = currentSO.healPower;
        shieldPower = currentSO.shieldPower;
        maxHealth = currentSO.maxHealth;

        currentMaskSprite = allMasksSprites[(int)mask];
        uiMask.sprite = currentMaskSprite;

        hpBar.text = "HP: " + maxHealth.ToString();
        shBar.text = "SH: " + shield.ToString();
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
        ChangePreviewImage();
    }

    public void ChangePreviewImage()
    {
        switch (previewAttack)
        {
            case EnemyAttack.Attack:
                previewImage.sprite = attackPreview;
                break;
            case EnemyAttack.Heal:
                previewImage.sprite = healPreview;
                break;
            case EnemyAttack.Shield:
                previewImage.sprite = shieldPreview;
                break;
            default:
                break;
        }
        
    }

    public void Heal()
    {
        AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.heal);
        if (debuff == NegativeEffect.freeze)
             health += healPower / 2;
        else 
            health += healPower;
        health = Math.Clamp(health, -1, maxHealth);
        UpdateUI();
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

    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.hurt);
        Debug.Log("DAMAGE DONE" + damage);
        Debug.Log("HEATH" + health);

        int remainingDamage = damage - shield;
        remainingDamage = Math.Clamp(remainingDamage, 0, 999);

        shield -= damage;
        shield = Math.Clamp(shield, 0, 999);
        health -= remainingDamage;
        
        UpdateUI();
        if (health <= 0)
            Die();

        health = Math.Clamp(health, 0, 999);
    }

    public void UpdateUI()
    {
        hpBar.text = "HP: " + health.ToString();
        shBar.text = "SP: " + shield.ToString();
        hpJauge.fillAmount = (float)health / (float)maxHealth;
    }

    public void ChangeState()
    {
        mask = nextMask;

        currentMaskSprite = allMasksSprites[(int)mask];
        uiMask.sprite = currentMaskSprite;
        uiMaskShadow.sprite = currentMaskSprite;
    }


    public void Shield()
    {
        AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.shield);
        if (debuff == NegativeEffect.freeze)
            health += shieldPower / 2;
        else 
            shield += shieldPower;
    }

    public void Special()
    {
        Debug.Log("It's a special attack!");
    }


    public void Die()
    {
        level++;
        NextLevel?.Invoke();
    }

    public void DEBUGInfoEnnemy()
    {
        Debug.Log($"Enemy : {name}!");
        Debug.Log($"Current Health : {health}!");
        Debug.Log($"Current Shield : {shield}!");
        Debug.Log($"Current Mask : {mask}!");
        Debug.Log($"Current Power : {damage}!");
        Debug.Log($"Current Debuff : {debuff}!");
    }
}
