using UnityEngine;

public class MonstronDisplay : MonoBehaviour
{
    public Monstron monstron;
    public SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer.sprite = monstron.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
