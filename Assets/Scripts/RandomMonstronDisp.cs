using UnityEngine;
using UnityEngine.UI;
public class RandomMonstronDisp : MonoBehaviour
{
    public Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img.sprite = DataObj.inst.monstrons[Random.Range(0, DataObj.monstronCount)].front;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
