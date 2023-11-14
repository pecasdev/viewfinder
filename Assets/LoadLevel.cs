using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("level1_new", LoadSceneMode.Single);
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect", LoadSceneMode.Single);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("level1_new", LoadSceneMode.Single);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("ArchitectDemo", LoadSceneMode.Single);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("level3", LoadSceneMode.Single);
    }
}