using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public int currentLevel = 0;
    public int[] levels = { 0, 1, 2 };
    // public double[] SOLUTION_POS = { -1.25443, -0.69, -0.398 };
    // public Vector3 SOLUTION_POS = new Vector3(-9.73f, 1.01f, -3.07f);
    // public Vector3 SOLUTION_ANGLE = new Vector3(27.25f, 179f, 0f);
    public GameObject LevelText;
    public GameObject TutorialText;
    public GameObject Prompt1;
    public GameObject Prompt2;
    public GameObject Prompt3;

    public delegate void OnLevelChangeHandler();
    public static GameManager Instance
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

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        UpdateLevel();
    }

    public void UpdateLevel()
    {
        if (currentLevel == 0)
        {
            LevelText.GetComponent<TextMeshProUGUI>().text = "Level 1";
        }
        else if (currentLevel == 1)
        {
            LevelText.GetComponent<TextMeshProUGUI>().text = "Level 2";
            TutorialText.SetActive(false);
            Prompt1.SetActive(false);
            SetCameraPosition(new Vector3(-6.33f, 2.5f, 5.82f));
            SetCameraAngle(new Vector3(33.2f, 308f, 0f));

        }
        else if (currentLevel == 2)
        {
            LevelText.GetComponent<TextMeshProUGUI>().text = "Level 3";
            Prompt2.SetActive(false);
            SetCameraPosition(new Vector3(1, 2, 3));
            SetCameraAngle(new Vector3(1, 2, 3));
        }
        else if (currentLevel > 2)
        {
            Prompt3.SetActive(false);
            LevelText.GetComponent<TextMeshProUGUI>().text = "You Win!";
        }
        currentLevel++;
    }

        public void SetCameraPosition(Vector3 camPos)
        {
        // SOLUTION_POS = camPos;
        }

    public void SetCameraAngle(Vector3 camAng)
    {
        // SOLUTION_ANGLE = camAng;
    }

public void PrintStatus()
    {
        Debug.Log("Test");
    }
}