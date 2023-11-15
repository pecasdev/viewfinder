using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class FTUEManager : MonoBehaviour
{
    private static FTUEManager instance = null;
    public GameObject movementFTUE;
    public GameObject cameraFTUE;
    public GameObject albumFTUE;
    public GameObject level1MechanicFTUE;
    public GameObject level2MechanicFTUE;
    public GameObject level3MechanicFTUE;

    public static FTUEManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Game Manager is NULL");
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartFTUE(GameState gameState)
    {
        if (gameState == GameState.MovementFTUE)
        {
            movementFTUE.SetActive(true);
        }
        else if (gameState == GameState.CameraFTUE)
        {
            cameraFTUE.SetActive(true);
        }
        else if (gameState == GameState.AlbumFTUE)
        {
            albumFTUE.SetActive(true);
        }
        else if (gameState == GameState.Level1MechanicFTUE)
        {
            level1MechanicFTUE.SetActive(true);
        }
        else if (gameState == GameState.Level2MechanicFTUE)
        {
            level2MechanicFTUE.SetActive(true);
        }
        else if (gameState == GameState.Level3MechanicFTUE)
        {
            level3MechanicFTUE.SetActive(true);
        }
    }

    internal void CheckFTUE()
    {
        DisableAllFtues();
        Debug.Log("Checking for FTUE");
        // Want to check if which ftues have not been completed. Initiate the first uncompleted one.
        if (PlayerPrefs.GetInt("movementFTUEComplete", 0) == 0)
        {
            Debug.Log("Starting MovementFTUE");
            StartFTUE(GameState.MovementFTUE);
        }
        else if (PlayerPrefs.GetInt("cameraFTUEComplete", 0) == 0)
        {
            StartFTUE(GameState.CameraFTUE);
        }
        else if (PlayerPrefs.GetInt("albumFTUEComplete", 0) == 0)
        {
            StartFTUE(GameState.AlbumFTUE);
        }
        else if (GameManager.Instance.currentLevel == 1 && PlayerPrefs.GetInt("level1MechanicFTUE", 0) == 0)
        {
            StartFTUE(GameState.Level1MechanicFTUE);
        }
        else if (GameManager.Instance.currentLevel == 2 && PlayerPrefs.GetInt("level2MechanicFTUE", 0) == 0)
        {
            StartFTUE(GameState.Level2MechanicFTUE);
        }
        else if (GameManager.Instance.currentLevel == 3 && PlayerPrefs.GetInt("level3MechanicFTUE", 0) == 0)
        {
            StartFTUE(GameState.Level3MechanicFTUE);
        }
        else
        {
            Debug.Log("All FTUEs complete");
        }
    }
    public void DisableAllFtues()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

}