using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoadScene()
    {
        SceneManager.LoadScene("level1", LoadSceneMode.Single);
    }
}