using UnityEngine;

[CreateAssetMenu(fileName = "MaskSO", menuName = "Scriptable Objects/MaskSO")]
public class MaskSO : ScriptableObject
{
    public MaskState maskState;
    public int damageMultipler;
    public int healMultiplier;
    public int shieldMultiplier;
}
