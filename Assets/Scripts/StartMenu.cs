using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public string gameSceneName = "SampleScene"; 

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
