using UnityEngine;

public enum SkinType
{
    Color,
    Effect,
    Epic,
}

[CreateAssetMenu(fileName = "Skins", menuName = "Scriptable Objects/Skins")]
public class Skins : ScriptableObject
{
    public string skinID;
    public string effectName;
    public SkinType skinType;
    public Color unlockedColor;   // shopban mutatjuk
    public int price;
    public bool unlockedByDefault = false;
    public GameObject effectPrefab; // Ha Effect típus
    public GameObject epicEffectPrefab;
    
}
