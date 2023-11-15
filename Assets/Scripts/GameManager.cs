using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public Sprite[] promptSprites;
    public TextAsset levelJson;
    private int solvedPrompts = 0;
    private int currentStage = 0;
    private List<Solution> solutions = new();
    private List<Prompt> prompts = new List<Prompt>();
    public GameState currentGameSate;
    public int currentLevel;
    public GameObject controlsUI;

    public enum StageOrder
    {
        Previous,
        Next
    }

    public enum GameState
    {
        MovementFTUE,
        CameraFTUE,
        AlbumFTUE,
        Level1MechanicFTUE,
        Level2MechanicFTUE,
        Level3MechanicFTUE,
        Playing,
        PausedMenu
    }
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

    public int SolvedPrompts { get => solvedPrompts; set => solvedPrompts = value; }
    public List<Prompt> Prompts { get => prompts; set => prompts = value; }
    public int CurrentStage { get => currentStage; set => currentStage = value; }

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

        int i = 0;
        foreach (SolutionText solutionText in solutionsText.solutions)
        {
            Debug.Log("Position Solution: " + solutionText.posSolutionText + " , Camera Position: " + solutionText.angleSolutionText);
            Solution solution = new Solution();
            solution.SetSolution(solutionText.posSolutionText, solutionText.angleSolutionText);
            solutions.Add(solution);

            Prompt newPrompt = new(false, solutionText.descriptionText, promptSprites[i]);
            prompts.Add(newPrompt);
            i += 1;
        }
    }

    private void Start()
    {
        currentGameSate = GameState.Playing;
        PhotoAlbumManager.Instance.UpdatePhotoAlbum();
        PromptPreviewManager.Instance.UpdatePromptPreview(false);
        FTUEManager.Instance.CheckFTUE();
    }

    public void PromptSolved()
    {
        if (prompts[currentStage].IsSolved == false)
        {
            // Disable hint after tutorial stage
            SolvedPrompts++;
            prompts[currentStage].IsSolved = true;
            prompts[currentStage].SolvedImage = HeldPhotoController.Instance.GetHeldPhotoImage();
            // Level Complete Logic
            if (SolvedPrompts >= solutions.Count)
            {
                //PhotoAlbumManager.Instance.UpdatePhotoAlbum();
                if (currentLevel == 1)
                {
                    PlayerPrefs.SetInt("level1Complete", 1);
                }
                else if (currentLevel == 2)
                {
                    PlayerPrefs.SetInt("level2Complete", 1);
                }
                else if (currentLevel == 3)
                {
                    PlayerPrefs.SetInt("level3Complete", 1);
                }
                PromptPreviewManager.Instance.UpdatePromptPreview(true);
            }
            else
            {
                // Adjust this to set the next prompt (unfinished prompt)
                // currentStage = GetNextUnfinishedPromptIndex();
                //
                //PhotoAlbumManager.Instance.UpdatePhotoAlbum();
                PromptPreviewManager.Instance.UpdatePromptPreview(false);
            }
        }
    }

    public Vector3 GetPositionSolution()
    {
        return solutions[CurrentStage].GetPositionSolution();
    }
    public Vector3 GetAngleSolution()
    {
        return solutions[CurrentStage].GetAngleSolution();
    }

    public void ChangeStage(StageOrder stageOrder)
    {
        if (stageOrder == StageOrder.Previous)
        {
            if (currentStage == 0)
            {
                currentStage = solutions.Count - 1;
            }
            else
            {
                currentStage--;
            }
        }
        else if (stageOrder == StageOrder.Next)
        {
            if (currentStage == solutions.Count - 1)
            {
                currentStage = 0;
            }
            else
            {
                currentStage++;
            }
        }

        PhotoAlbumManager.Instance.UpdatePhotoAlbum();
        if (SolvedPrompts >= solutions.Count)
        {
            PromptPreviewManager.Instance.UpdatePromptPreview(true);
        }
        else
        {
            PromptPreviewManager.Instance.UpdatePromptPreview(false);
        }
    }

    public Prompt GetCurrentPrompt()
    {
        return prompts[CurrentStage];
    }

    public int GetTotalStages()
    {
        return solutions.Count;
    }

    public int GetNextUnfinishedPromptIndex()
    {
        int count = 0;
        foreach (Prompt prompt in prompts)
        {
            if (!prompt.IsSolved)
            {
                return count;
            }
            count++;
        }

        return -1;
    }
}

public class Solution
{
    Vector3 posSolution;
    Vector3 angleSolution;
    string description;

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
    public string descriptionText;
}

[System.Serializable]
public class SolutionsText
{
    public SolutionText[] solutions;
}

public class Prompt
{
    bool isSolved;
    string descriptionText;
    Sprite promptImage;
    Sprite solvedImage;

    public Prompt(bool isSolved, string descriptionText, Sprite promptImage)
    {
        this.isSolved = isSolved;
        this.descriptionText = descriptionText;
        this.promptImage = promptImage;
        this.solvedImage = promptImage;
    }

    public string DescriptionText { get => descriptionText; set => descriptionText = value; }
    public bool IsSolved { get => isSolved; set => isSolved = value; }
    public Sprite PromptImage { get => promptImage; set => promptImage = value; }
    public Sprite SolvedImage { get => solvedImage; set => solvedImage = value; }
}
