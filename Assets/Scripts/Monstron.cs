using UnityEngine;

[CreateAssetMenu(fileName = "Monstron", menuName = "Scriptable Objects/Monstron")]
public class Monstron : ScriptableObject
{
    public enum Type {
        T1,
        T2,
        T3
    }
    public string monster_name;
    public int health;
    public int speed;
    public int defense;
    public int attack;
    
    public Type monster_type;
    public Sprite sprite;
    
}
