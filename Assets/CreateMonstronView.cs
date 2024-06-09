using UnityEngine;

public class CreateMonstronView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public GameObject monstronView;
    void Start()
    {

        foreach (Monstron m in DataObj.inst.monstrons)
        {
            monstronView.GetComponent<MonstronDisplay>().monstron = m;
            Instantiate(monstronView, gameObject.transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
