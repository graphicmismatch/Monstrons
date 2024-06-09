using UnityEngine;
using UnityEngine.UI;

public class MonstronDisplay : MonoBehaviour
{
    public Monstron monstron;
    public Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.sprite = monstron.front;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
