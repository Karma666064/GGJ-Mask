using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public new string name;
    public MaskState mask;
    public int health;
    public int maxHealth;
    public int shield;
    public int damage;
    public int healPower;
    public int shieldPower;
    public EnemyAttack[] pattern;
    public bool hasSpecial;
}
