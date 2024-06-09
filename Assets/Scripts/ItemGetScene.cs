using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemGetScene : MonoBehaviour
{
    public Image monstron;
    public TMP_Text caught;
    private int item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Random.InitState(DataObj.seed);
        Random.Range(0, 3);
        item = Random.Range(0, DataObj.itemCount);
        monstron.sprite = DataObj.inst.items[item].spr;
        caught.text = $"You Obtained {DataObj.inst.items[item].itemName}!";
        DataObj.AddItem(item);
    }
}
