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
    private bool dpad_v_button_pressed = false;
    private bool dpad_h_button_pressed = false;
    private bool b_button_pressed = false;
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
        if (PauseMenu.gameIsPaused) return;
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


        // xbox controls
        // dpad vertical to open/close album
        float dpad_v_Value = Input.GetAxis("DPAD_v Windows");
        if (dpad_v_Value == 0)
        {
            dpad_v_button_pressed = false;
        }

        if (Input.GetAxis("DPAD_v Windows") == -1 && !dpad_v_button_pressed)
        {
            dpad_v_button_pressed = true;
            OpenPhotoAlbum();
        }
        else if (Input.GetAxis("DPAD_v Windows") == 1 && !dpad_v_button_pressed)
        {
            dpad_v_button_pressed = true;
            ClosePhotoAlbum();
        }

        // dpad horizontal to cycle prompts
        float dpad_h_Value = Input.GetAxis("DPAD_h Windows");
        if (dpad_h_Value == 0)
        {
            dpad_h_button_pressed = false;
        }

        if (Input.GetAxis("DPAD_h Windows") == -1 && !dpad_h_button_pressed)
        {
            Debug.Log("DPAD_h left");
            dpad_h_button_pressed = true;
            ChangeStage(GameManager.StageOrder.Previous);
        }
        else if (Input.GetAxis("DPAD_h Windows") == 1 && !dpad_h_button_pressed)
        {
            Debug.Log("DPAD_h right");
            dpad_h_button_pressed = true;
            ChangeStage(GameManager.StageOrder.Next);
        }

        // b button to exit prompt
        if (!PauseMenu.gameIsPaused)
        {
            float b_button_val = Input.GetAxis("Xbox_B_Button");
            if (b_button_val == 0)
            {
                b_button_pressed = false;
            }

            if (Input.GetAxis("Xbox_B_Button") == 1 && !b_button_pressed)
            {
                b_button_pressed = true;
                ClosePhotoAlbum();
            }
        }
        

        // Y button to teleport player between worlds
        //float y_button_val = Input.GetAxis("Xbox_Y_Button");
        //if (y_button_val == 0)
        //{
        //    y_button_pressed = false;
        //}

        //if (Input.GetAxis("Xbox_Y_Button") == 1 && !y_button_pressed)
        //{
        //    y_button_pressed = true;
        //}


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
            promptDescription.text = "Maybe if I retook this phantom photo I'll learn about what happened...";
            // decrease alpha of the sprite
            Color promptCol = promptImage.color;
            //promptCol.a = 0.75f;
            promptCol.r = 0.8f;
            promptCol.g = 0.8f;
            promptCol.b = 0.8f;
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
            //PromptPreviewManager.Instance.TogglePromptPreview();
        }

    }

    public void OpenPhotoAlbum()
    {
        if (!promptImageBig.activeInHierarchy && !photoAlbumContainer.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(true);
        }
        else if (photoAlbumContainer.activeInHierarchy)
        {
            TogglePromptSize();
        }
    }

    public void ClosePhotoAlbum()
    {
        if (!promptImageBig.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(false);
        }
        else if (promptImageBig.activeInHierarchy)
        {
            TogglePromptSize();
        }
    }

    private void TogglePromptSize()
    {
        if (photoAlbumContainer.activeInHierarchy && !promptImageBig.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(false);
            promptImageBig.SetActive(true);
            PromptPreviewManager.Instance.TogglePromptPreview();
            Color promptCol = promptImageBig.GetComponent<Image>().color;
            promptCol.a = 0.5f;
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
            PromptPreviewManager.Instance.TogglePromptPreview();
        }

    }
}