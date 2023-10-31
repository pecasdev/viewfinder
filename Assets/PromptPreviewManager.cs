using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptPreviewManager : MonoBehaviour
{
    public Image promptImage;
    public GameObject stageText;
    public GameObject hintText;
    public GameObject promptPreviewContainer;

    private static PromptPreviewManager instance = null;
    public static PromptPreviewManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("PromptManager is NULL");
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }

    public void TogglePromptPreview()
    {
        promptPreviewContainer.SetActive(!promptPreviewContainer.activeInHierarchy);
    }

    public void HidePromptPreview()
    {
        promptPreviewContainer.SetActive(false);
    }

    public void ShowPromptPreview()
    {
        promptPreviewContainer.SetActive(true);
    }

    public void UpdatePromptPreview(bool gameComplete)
    {
        if (gameComplete)
        {
            stageText.GetComponent<TextMeshProUGUI>().color = Color.green;
            hintText.SetActive(true);
            hintText.GetComponent<TextMeshProUGUI>().text = "You Win! Loading Next Level...";
        }
        else
        {
            hintText.SetActive(false);
            promptImage.sprite = GameManager.Instance.GetCurrentPrompt().PromptImage;
            if (GameManager.Instance.GetCurrentPrompt().IsSolved)
            {
                Color promptCol = promptImage.color;
                promptCol.a = 1;
                promptCol.r = 1;
                promptCol.g = 1;
                promptCol.b = 1;
                promptImage.GetComponent<Image>().color = promptCol;
            }
            else
            {
                Color promptCol = promptImage.color;
                // promptCol.a = 0.75f;
                promptCol.r = 0.8f;
                promptCol.g = 0.8f;
                promptCol.b = 0.8f;
                promptImage.GetComponent<Image>().color = promptCol;
            }
        }
        stageText.GetComponent<TextMeshProUGUI>().text = "Prompts Completed: " + (GameManager.Instance.SolvedPrompts) + "/" + GameManager.Instance.GetTotalStages();
    }
}
