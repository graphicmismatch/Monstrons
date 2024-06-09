using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public enum MonType
{
    Fire,Water,Dark,Energy,Rock,Poison
}
[CreateAssetMenu(fileName = "Monstron", menuName = "Scriptable Objects/Monstron")]
public class Monstron : ScriptableObject
{

    public string monster_name;
    public int health;
    public int speed;
    public int defense;
    public int attack;

    public MonType monster_type;
    public Sprite back;
    public Sprite front;
}
