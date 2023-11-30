using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
