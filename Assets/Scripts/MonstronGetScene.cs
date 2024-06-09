using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MonstronGetScene : MonoBehaviour
{
    public Image monstron;
    public TMP_Text caught;
    private MonstronData md;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        md = NewMonstronSpawner.spawn();
        monstron.sprite = DataObj.inst.monstrons[md.id].front;
        caught.text = $"You Caught {DataObj.inst.monstrons[md.id].monster_name}!";
        DataObj.AddMon(md);
    }

}
