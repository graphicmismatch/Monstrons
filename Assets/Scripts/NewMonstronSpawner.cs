using UnityEngine;

public class NewMonstronSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static MonstronData spawn() {
        Random.InitState(DataObj.seed);
        Random.Range(0, 3);
        MonstronData md = new MonstronData();
        int id = Random.Range(0, DataObj.monstronCount);
        md.id = id;
        md.move1 = Random.Range(0, DataObj.moveCount);
        md.move2 = Random.Range(0, DataObj.moveCount);
        md.level = 1;
        return md;
    }
    
}
