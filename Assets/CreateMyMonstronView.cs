using UnityEngine;

public class CreateMyMonstronView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject monstronView;
    void Start()
    {
        foreach (MonstronData m in DataObj.myMonstrons)
        {
            monstronView.GetComponent<MonstronDisplay>().monstron = DataObj.inst.monstrons[m.id];
            Instantiate(monstronView, gameObject.transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
