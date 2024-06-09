using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChan : MonoBehaviour
{

    public string sc;


    public void changeNoAnim()
    {
        SceneManager.LoadScene(sc);

    }
}
