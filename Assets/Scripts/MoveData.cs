using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Scriptable Objects/Move")]
public class MoveData : ScriptableObject
{
    public string movename;
    public int mindamage;
    public int maxdamage;
    
    
    public int minHeal;
    public int maxHeal;


    public MonType type;
    public int critChance;
    public int poisonChance;
    public bool isLifedrain;
}
