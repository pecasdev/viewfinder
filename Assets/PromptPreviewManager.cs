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
    private Animator promptPreviewAnimator;
    private bool promptEnlarged = false;
    [SerializeField]
    private Image promptStatusImg;
    [SerializeField]
    private Sprite checkmarkSprite;
    [SerializeField]
    private Sprite xmarkSprite;

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
        promptPreviewAnimator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (GameManager.Instance.currentGameSate != GameManager.GameState.PausedMenu)
        {
            if (Input.GetButtonUp("Jump"))
            {
                MinimizePrompt();
            }
            if (Input.GetButtonDown("Jump"))
            {
                EnlargePrompt();
            }
        }
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

    private void TogglePromptSize()
    {
        if (promptEnlarged)
        {
            MinimizePrompt();
        } else
        {
            EnlargePrompt();
        }
    }

    private void EnlargePrompt()
    {
        if (!promptEnlarged)
        {
            promptPreviewAnimator.Play("EnlargePrompt");
            promptEnlarged = true;
        }
    }

    private void MinimizePrompt()
    {
        if (promptEnlarged)
        {
            promptPreviewAnimator.Play("MinimizePrompt");
            promptEnlarged = false;
        }
    }

    public void UpdatePromptPreview(bool gameComplete)
    {
        if (gameComplete)
        {
            stageText.GetComponent<TextMeshProUGUI>().color = Color.green;
            hintText.SetActive(true);
            //hintText.GetComponent<TextMeshProUGUI>().text = "You Win! Loading Next Level...";
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
                promptStatusImg.sprite = checkmarkSprite;
            }
            else
            {
                Color promptCol = promptImage.color;
                // promptCol.a = 0.75f;
                promptCol.r = 0.8f;
                promptCol.g = 0.8f;
                promptCol.b = 0.8f;
                promptImage.GetComponent<Image>().color = promptCol;
                promptStatusImg.sprite = xmarkSprite;
            }
        }
        stageText.GetComponent<TextMeshProUGUI>().text = "Prompts Completed: " + (GameManager.Instance.SolvedPrompts) + "/" + GameManager.Instance.GetTotalStages();
    }
}
