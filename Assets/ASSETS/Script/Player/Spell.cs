using UnityEngine;

[System.Serializable]
public class Spell
{
    public string spellName;
    public string keyword;  // "fireball"
    public GameObject prefab;
    public float cooldown = 1f;
}
