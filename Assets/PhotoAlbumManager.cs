using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbumManager : MonoBehaviour
{
    private static PhotoAlbumManager instance = null;
    private bool isPromptSolved = false;
    [SerializeField]
    private Image promptImage;
    [SerializeField]
    private GameObject promptImageBig;
    [SerializeField]
    private TextMeshProUGUI promptDescription;
    [SerializeField]
    private GameObject photoAlbumContainer;
    [SerializeField]
    private TextMeshProUGUI promptLabelText;
    public static PhotoAlbumManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("PhotoAlbumManager is NULL");
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            TogglePhotoAlbum();
        }
        if (Input.GetKeyDown("n"))
        {
            TogglePromptSize();
        }
        if (Input.GetKeyDown("9")) { ChangeStage(GameManager.StageOrder.Previous); }
        if (Input.GetKeyDown("0")) { ChangeStage(GameManager.StageOrder.Next); }
    }

    public void ChangeStage(GameManager.StageOrder stageOrder)
    {
        if (photoAlbumContainer.activeInHierarchy && !promptImageBig.activeInHierarchy)
        {
            GameManager.Instance.ChangeStage(stageOrder);
        }
    }

    // Call this whenever stage is changed
    public void UpdatePhotoAlbum()
    {
        Prompt currentPrompt = GameManager.Instance.GetCurrentPrompt();
        promptImage.sprite = currentPrompt.PromptImage;
        promptImageBig.GetComponent<Image>().sprite = currentPrompt.PromptImage;
        isPromptSolved = currentPrompt.IsSolved;
        if (isPromptSolved)
        {
            promptDescription.text = currentPrompt.DescriptionText;
            // increase alpha of the sprite (phantom photo)
            Color promptCol = promptImage.color;
            //promptCol.a = 1;
            promptCol.r = 1;
            promptCol.g = 1;
            promptCol.b = 1;
            promptImage.GetComponent<Image>().color = promptCol;
        }
        else
        {
            // decrease alpha of the sprite
            Color promptCol = promptImage.color;
            //promptCol.a = 0.75f;
            promptCol.r = 0.5f;
            promptCol.g = 0.5f;
            promptCol.b = 0.9f;
            promptImage.GetComponent<Image>().color = promptCol;
        }
        promptLabelText.text = "Prompt " + (GameManager.Instance.CurrentStage + 1) + "/" + GameManager.Instance.GetTotalStages();
    }
    // Connect this to control script
    public void TogglePhotoAlbum()
    {
        // Ignore if the prompt is maximized
        if (!promptImageBig.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(!photoAlbumContainer.activeInHierarchy);
            PromptPreviewManager.Instance.TogglePromptPreview();
        }

    }

    public void OpenPhotoAlbum()
    {
        if (!promptImageBig.activeInHierarchy && !photoAlbumContainer.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(true);
        }
        //else if (photoAlbumContainer.activeInHierarchy)
        //{
        //    photoAlbumContainer.SetActive(false);
        //    promptImageBig.SetActive(true);
        //}
    }

    public void ClosePhotoAlbum()
    {
        if (!promptImageBig.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(false);
        }
        else if (promptImageBig.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(true);
            promptImageBig.SetActive(false);
        }
    }

    private void TogglePromptSize()
    {
        if (photoAlbumContainer.activeInHierarchy && !promptImageBig.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(false);
            promptImageBig.SetActive(true);
            Color promptCol = promptImageBig.GetComponent<Image>().color;
            //promptCol.a = 0.3f;
            if (isPromptSolved)
            {
                promptCol.r = 1;
                promptCol.g = 1;
                promptCol.b = 1;
            }
            else
            {
                promptCol.r = 0.5f;
                promptCol.g = 0.5f;
                promptCol.b = 0.9f;
            }
            promptImageBig.GetComponent<Image>().color = promptCol;
        }
        else if (promptImageBig.activeInHierarchy && !photoAlbumContainer.activeInHierarchy)
        {
            promptImageBig.SetActive(false);
            photoAlbumContainer.SetActive(true);
        }

    }
}