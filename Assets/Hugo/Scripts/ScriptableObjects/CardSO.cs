using UnityEngine;

[CreateAssetMenu(fileName = "Cards", menuName = "Scriptable Objects/CreateCards")]
public class CardSO: ScriptableObject
{ 
    public CardType type;
    public MaskState mask;
    public GameObject currentSprite;
    public GameObject allSprites;
    public string[] allNames;
    public string currentName;
    public string currentDescription;
    public string[] allDescriptions;
    public int cost;
    public int damage;
    public int heal;
    public int shield;
    public CardEffect currentEffect;
    public CardEffect[] allEffects;

}
