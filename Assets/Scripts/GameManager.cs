using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public GameObject stageText;
    public GameObject hintText;
    public Image promptGameObject;
    public Sprite[] promptSprites;
    public TextAsset levelJson;
    private int currentStage = 0;
    private List<Solution> solutions = new();


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
        instance = null;
    }

    private void Awake()
    {
        instance = this;
        ParseLevelJson();
    }

    private void ParseLevelJson()
    {
        SolutionsText solutionsText = JsonUtility.FromJson<SolutionsText>(levelJson.text);

        foreach (SolutionText solutionText in solutionsText.solutions)
        {
            Debug.Log("Position Solution: " + solutionText.posSolutionText + " , Camera Position: " + solutionText.angleSolutionText);
            Solution solution = new Solution();
            solution.SetSolution(solutionText.posSolutionText, solutionText.angleSolutionText);
            solutions.Add(solution);
        }
    }

    private void Start()
    {
        // Temporary home for this logic
        stageText.GetComponent<TextMeshProUGUI>().text = "Stage " + (currentStage + 1);
        hintText.GetComponent<TextMeshProUGUI>().text = "Press P to take a photo!";
        SetPromptSprite();
    }

    public void UpdateStage()
    {
        currentStage++;
        // Disable hint after tutorial stage
        hintText.SetActive(false);
        // Level Complete Logic
        if (currentStage >= solutions.Count)
        {
            stageText.GetComponent<TextMeshProUGUI>().text = "You Win!";
            return;
        }
        // Update Stage
        else
        {
            stageText.GetComponent<TextMeshProUGUI>().text = "Stage " + (currentStage + 1);
            SetNextSolution();
        }
        SetPromptSprite();
    }

    public Vector3 GetPositionSolution()
    {
        return solutions[currentStage].GetPositionSolution();
    }
    public Vector3 GetAngleSolution()
    {
        return solutions[currentStage].GetAngleSolution();
    }
    private void SetNextSolution()
    {

    }

    private void SetPromptSprite()
    {
        promptGameObject.sprite = promptSprites[currentStage];
    }

    public int GetCurrentStage() { return currentStage; }
}

public class Solution
{
    Vector3 posSolution;
    Vector3 angleSolution;

    public void SetSolution(string posSolutionText, string angleSolutionText)
    {
        string[] splitPosText = posSolutionText.Split(',');
        string[] splitAngleText = angleSolutionText.Split(",");

        posSolution = new Vector3(float.Parse(splitPosText[0]), float.Parse(splitPosText[1]), float.Parse(splitPosText[2]));
        angleSolution = new Vector3(float.Parse(splitAngleText[0]), float.Parse(splitAngleText[1]), float.Parse(splitAngleText[2]));
    }

    public Vector3 GetPositionSolution()
    {
        return posSolution;
    }

    public Vector3 GetAngleSolution()
    {
        return angleSolution;
    }
}

[System.Serializable]
public class SolutionText
{
    public string posSolutionText;
    public string angleSolutionText;
}

[System.Serializable]
public class SolutionsText
{
    public SolutionText[] solutions;
}