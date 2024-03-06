using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public string description;
    public int cost;
    public Sprite icon;
    public GameObject spellPrefab;
}


