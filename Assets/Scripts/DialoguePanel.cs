using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
public class DialoguePanel : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text dlog;
    public static DialoguePanel inst;
    public bool isTyping;
    public bool skip;
    public string curr;
    public List<string> squeue;
    public float timeBetweenCharacters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        squeue = new List<string>();
        isTyping = false;
        skip = false;
        inst = this;
        curr = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (curr == null && squeue.Count > 0) {
            dlog.text = "";
            panel.SetActive(true);
            curr = squeue[0];
            squeue.RemoveAt(0);
            StartCoroutine(slowType(curr, timeBetweenCharacters));
        }
    }

    public static void addDialogue(string s) {
        inst.squeue.Add(s);
    }
    public IEnumerator slowType(string s, float tBWc)
    {
        isTyping = true;
        dlog.text = "";
        foreach (char c in s)
        {
            if (skip) {
                dlog.text = s;
                break;
            }
            dlog.text += c.ToString();
            yield return new WaitForSeconds(tBWc);
        }
        isTyping = false;
        skip = false;

    }
    public void onClick() {
        if (isTyping) {
            skip = true;
            return;
        }

        if (squeue.Count > 0)
        {
            curr = squeue[0];
            squeue.RemoveAt(0);
            StartCoroutine(slowType(curr, timeBetweenCharacters));
        }
        else {
            dlog.text = "";
            curr = null;
            panel.SetActive(false);
        }
    }
}
