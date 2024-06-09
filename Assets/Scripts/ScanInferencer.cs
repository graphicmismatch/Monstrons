using UnityEngine;
using ZXing;
using System.IO;
using System.Collections;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class ScanInferencer : MonoBehaviour
{
    public Animator anim;
    string path;
    private string scannedt;
    private List<string> scanned;
    private string last;
    public TMP_Text t;
    public TMP_Text encounter;
    public GameObject prevScan;
    public GameObject Encounter;
    public bool jscanned = false;
    public string SceneToLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneToLoad = "";
        anim.SetBool("open", false);
        prevScan.SetActive(false);
        Encounter.SetActive(false);
        path = Application.persistentDataPath + "/GameSaves/scans.file";
        print(path);
        if (File.Exists(path))
        {
            scannedt = Base64Decode(File.ReadAllText(path));
        }
        else {
            if (!Directory.Exists(Application.persistentDataPath + "/GameSaves")) {
                Directory.CreateDirectory(Application.persistentDataPath + "/GameSaves");
            }
            File.Create(path);
            scannedt = "";
        }
        scanned = new List<string>(scannedt.Split(','));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onScan(Result r) {
        anim.SetBool("open", true);
        t.text = r.Text;
        last = r.Text;
        if (!scanned.Contains(r.Text))
        {
            scannedt += r.Text+",";
            scanned.Add(r.Text);
            SaveScanned();
            doEncounter(r);
        }
        else {
            prevScan.SetActive(true);
        }
    }

    public void doEncounter(Result r) {
        Encounter.SetActive(true);
        int seed = r.Text.GetHashCode(StringComparison.OrdinalIgnoreCase);
        UnityEngine.Random.InitState(seed);
        int c = UnityEngine.Random.Range(0,3);
        DataObj.seed = seed;
        if (c == 0)
        {
            encounter.text = "You found a Monstron!";
            SceneToLoad = "GetMonstronScene";
            //get mon
        }
        else if (c == 1)
        {
            encounter.text = "You found an Item!";
            SceneToLoad = "GetItemScene";
            // get item
        } 
        else if(c == 2) {
            encounter.text = "You have been challenged!";
            SceneToLoad = "BattleScene";
            //battle  
        }
    }
    public void onCopy() {
        UniClipboard.SetText(last);
    }
    public void onOpen()
    {
        try
        {
            Application.OpenURL(last);
        }
        catch {
            return;
        }
        
        
    }
    public void Accept() {
        SceneManager.LoadScene(SceneToLoad);
    }
    public void close() {
        prevScan.SetActive(false);
        Encounter.SetActive(false);
        anim.SetBool("open", false);
    }
    public void SaveScanned() {
        if (!File.Exists(path))
        {
            if (!Directory.Exists(Application.persistentDataPath + "/GameSaves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/GameSaves");
            }
            File.Create(path);
        }
        File.WriteAllText(path, Base64Encode(scannedt));
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
}
