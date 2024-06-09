using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Items")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int experienceGain;
    public Sprite spr;
    public int healthbuff;
    public int speedbuff;
    public int defensebuff;
    public int attackbuff;
}
