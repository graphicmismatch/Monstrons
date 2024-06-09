using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
public class DataObj : MonoBehaviour
{
    public static DataObj inst;
    public static int monstronCount;
    public static int moveCount;
    public static int itemCount;
    public Monstron[] monstrons;
    public MoveData[] moves;
    public ItemData[] items;
    public static int seed = -1;
    public static List<MonstronData> myMonstrons;
    public static List<int> myItems;
    public static int activeMon = -1;
    private string SavePath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SavePath = Application.persistentDataPath + "/GameSaves/save.file";
        if (File.Exists(SavePath))
        {
            if (File.ReadAllText(SavePath) != "")
            {
                PlayerData p = new PlayerData();
                p = (PlayerData)JsonConvert.DeserializeObject((File.ReadAllText(SavePath)));
                myMonstrons = new List<MonstronData>(p.myMonstrons);
                myItems = new List<int>(p.items);
                foreach (MonstronData md in myMonstrons) {
                    if (md.isActive) {
                        activeMon = myMonstrons.IndexOf(md);
                        break;
                    }
                }
            }
        }
        else
        {
            if (!Directory.Exists(Application.persistentDataPath + "/GameSaves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/GameSaves");
            }
            File.Create(SavePath);
            myMonstrons = new List<MonstronData>();
        }
        monstronCount = monstrons.Length;
        moveCount = moves.Length;
        DontDestroyOnLoad(this);
        if (inst == null) {
            inst = this;
        }
        if (inst != this) {
            Destroy(this);
        }
    }
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    public static void AddMon(MonstronData md) {
        myMonstrons.Add(md);
        if (activeMon == -1) {
            activeMon = myMonstrons.Count - 1;
            myMonstrons[myMonstrons.Count-1].isActive = true;
        }
        inst.Save();
    }
    public static void AddItem(int md)
    {
        myItems.Add(md);
        inst.Save();
    }

    public static void changeActive(int ind) {
        myMonstrons[ind].isActive = true;
        myMonstrons[activeMon].isActive = false;
        activeMon = ind;
    }
    public void Save() {
       
        if (!File.Exists(SavePath))
        {
            if (!Directory.Exists(Application.persistentDataPath + "/GameSaves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/GameSaves");
            }
            File.Create(SavePath);
        }
        PlayerData p = new PlayerData();
        p.items = myItems.ToArray();
        p.myMonstrons = myMonstrons.ToArray();
        File.WriteAllText(SavePath, Base64Encode(JsonConvert.SerializeObject(p)));
    }
    // Update is called once per frame

}
