using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbumManager : MonoBehaviour
{
    public AudioClip[] pageFlipSounds;
    public AudioClip bookOpenSound;
    public AudioClip bookCloseSound;
    private AudioSource audioSource;

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
    [SerializeField]
    private Image promptStatusImg;
    [SerializeField]
    private Sprite checkmarkSprite;
    [SerializeField]
    private Sprite xmarkSprite;
    private bool dpad_v_button_pressed = false;
    private bool dpad_h_button_pressed = false;
    private bool b_button_pressed = false;
    [SerializeField] GameObject glow;

    string DPAD_v;
    string DPAD_h;
    string Xbox_B_Button;

    private Animator _albumAnimator;

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
        _albumAnimator = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        switch (UnityEngine.Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                DPAD_h = "DPAD_h Windows";
                DPAD_v = "DPAD_v Windows";
                Xbox_B_Button = "Xbox_B_Button";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                DPAD_h = "DPAD_h Mac";
                DPAD_v = "DPAD_v Mac";
                Xbox_B_Button = "Xbox_B_Button Mac";
                break;
        }
    }

    private void playBookOpen()
    {
        audioSource.clip = bookOpenSound;
        audioSource.Play();
    }
    private void playBookClose()
    {
        audioSource.clip = bookCloseSound;
        audioSource.Play();
    }

    private void playPageFlipSound()
    {
        int index = UnityEngine.Random.Range(0, pageFlipSounds.Length);
        audioSource.clip = pageFlipSounds[index];
        audioSource.Play();
    }
    void Update()
    {
        if (GameManager.Instance.currentGameSate == GameManager.GameState.PausedMenu || (GameManager.Instance.currentGameSate != GameManager.GameState.AlbumFTUE && GameManager.Instance.currentGameSate != GameManager.GameState.Playing)) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TogglePhotoAlbum();
        }
        //if (Input.GetKeyDown("n"))
        //{
        //    TogglePromptSize();
        //}
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { ChangeStage(GameManager.StageOrder.Previous); playPageFlipSound(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { ChangeStage(GameManager.StageOrder.Next); playPageFlipSound(); }
        if (Input.GetKeyDown(KeyCode.B)) { StartCoroutine(ClosePhotoAlbum()); }


        // xbox controls
        // dpad vertical to open/close album

        switch (UnityEngine.Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                float dpad_v_Value = Input.GetAxis(DPAD_v);
                if (dpad_v_Value == 0)
                {
                    dpad_v_button_pressed = false;
                }

                if (Input.GetAxis(DPAD_v) == 1 && !dpad_v_button_pressed)
                {
                    dpad_v_button_pressed = true;
                    OpenPhotoAlbum();
                }
                else if (Input.GetAxis(DPAD_v) == -1 && !dpad_v_button_pressed)
                {
                    dpad_v_button_pressed = true;
                    StartCoroutine(ClosePhotoAlbum());
                }

                // dpad horizontal to cycle prompts
                float dpad_h_Value = Input.GetAxis(DPAD_h);
                if (dpad_h_Value == 0)
                {
                    dpad_h_button_pressed = false;
                }
                Debug.Log(Input.GetAxis(DPAD_h) + ","+ Input.GetAxis(DPAD_v));
                if (Input.GetAxis(DPAD_h) == -1 && !dpad_h_button_pressed)
                {
                    Debug.Log("DPAD_h left");
                    dpad_h_button_pressed = true;
                    ChangeStage(GameManager.StageOrder.Previous);
                    playPageFlipSound();
                }
                else if (Input.GetAxis(DPAD_h) == 1 && !dpad_h_button_pressed)
                {
                    Debug.Log("DPAD_h right");
                    dpad_h_button_pressed = true;
                    ChangeStage(GameManager.StageOrder.Next);
                    playPageFlipSound();
                }
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                dpad_v_Value = Input.GetAxis(DPAD_v);
                dpad_h_Value = Input.GetAxis(DPAD_h);
                if (dpad_v_Value == 0 && dpad_h_Value == -1)
                {
                    dpad_v_button_pressed = false;
                }

                if (dpad_h_Value == -1 && dpad_v_Value == 1 && !dpad_v_button_pressed)
                {
                    dpad_v_button_pressed = true;
                    OpenPhotoAlbum();
                }
                else if (dpad_h_Value == 1 && dpad_v_Value == -1 && !dpad_v_button_pressed)
                {
                    dpad_v_button_pressed = true;
                    StartCoroutine(ClosePhotoAlbum());
                }

                // dpad horizontal to cycle prompts
                
                if (dpad_v_Value == 0 && dpad_h_Value == -1)
                {
                    dpad_h_button_pressed = false;
                }
                Debug.Log(Input.GetAxis(DPAD_h) + ","+ Input.GetAxis(DPAD_v));
                if (dpad_v_Value == -1 && dpad_h_Value == -1 && !dpad_h_button_pressed)
                {
                    Debug.Log("DPAD_h left");
                    dpad_h_button_pressed = true;
                    ChangeStage(GameManager.StageOrder.Previous);
                    playPageFlipSound();
                }
                else if (dpad_h_Value == 1 && dpad_v_Value == 1 && !dpad_h_button_pressed)
                {
                    Debug.Log("DPAD_h right");
                    dpad_h_button_pressed = true;
                    ChangeStage(GameManager.StageOrder.Next);
                    playPageFlipSound();
                }
                break;
        }
        

        // b button to exit prompt
        if (GameManager.Instance.currentGameSate != GameManager.GameState.PausedMenu && (GameManager.Instance.currentGameSate == GameManager.GameState.AlbumFTUE || GameManager.Instance.currentGameSate == GameManager.GameState.Playing))
        {
            float b_button_val = Input.GetAxis(Xbox_B_Button);
            if (b_button_val == 0)
            {
                b_button_pressed = false;
            }

            if (Input.GetAxis(Xbox_B_Button) == 1 && !b_button_pressed)
            {
                b_button_pressed = true;
                StartCoroutine(ClosePhotoAlbum());
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
        isPromptSolved = currentPrompt.IsSolved;
        promptImageBig.GetComponent<Image>().sprite = currentPrompt.PromptImage;
        if (isPromptSolved)
        {
            promptImage.sprite = currentPrompt.SolvedImage;
            promptDescription.text = currentPrompt.DescriptionText;
            // increase alpha of the sprite (phantom photo)
            Color promptCol = promptImage.color;
            //promptCol.a = 1;
            promptCol.r = 1;
            promptCol.g = 1;
            promptCol.b = 1;
            promptImage.GetComponent<Image>().color = promptCol;
            promptStatusImg.sprite = checkmarkSprite;
            glow.SetActive(false);
        }
        else
        {
            promptImage.sprite = currentPrompt.PromptImage;
            promptDescription.text = "Maybe if I retook this phantom photo I'll learn about what happened...";
            // decrease alpha of the sprite
            Color promptCol = promptImage.color;
            //promptCol.a = 0.75f;
            promptCol.r = 0.8f;
            promptCol.g = 0.8f;
            promptCol.b = 0.8f;
            promptImage.GetComponent<Image>().color = promptCol;
            promptStatusImg.sprite = xmarkSprite;
            glow.SetActive(true);
        }
        promptLabelText.text = "Prompt " + (GameManager.Instance.CurrentStage + 1) + "/" + GameManager.Instance.GetTotalStages();
    }
    // Connect this to control script
    public void TogglePhotoAlbum()
    {
        // Ignore if the prompt is maximized
        if (!promptImageBig.activeInHierarchy)
        {
            if (photoAlbumContainer.activeInHierarchy)
            {
                StartCoroutine(ClosePhotoAlbum());
            }
            else
            {
                OpenPhotoAlbum();
            }
            //photoAlbumContainer.SetActive(!photoAlbumContainer.activeInHierarchy);
            //PromptPreviewManager.Instance.TogglePromptPreview();
        }

    }

    public void RevealCaption()
    {
        if (!promptImageBig.activeInHierarchy)
        {
            _albumAnimator.Play("RevealPhotoCaption");
        }
    }

    public void OpenPhotoAlbum()
    {
        playBookOpen();
        if (!promptImageBig.activeInHierarchy && !photoAlbumContainer.activeInHierarchy)
        {
            photoAlbumContainer.SetActive(true);
            if (_albumAnimator)
            {
                _albumAnimator.Play("OpenAlbum");
            }
        }
        //else if (photoAlbumContainer.activeInHierarchy)
        //{
        //    TogglePromptSize();
        //}
    }

    IEnumerator ClosePhotoAlbum()
    {
        playBookClose();
        if (!promptImageBig.activeInHierarchy)
        {
            if (_albumAnimator)
            {
                _albumAnimator.Play("CloseAlbum");
            }
            yield return new WaitForSeconds(1.0f);
            photoAlbumContainer.SetActive(false);
        }
        //else if (promptImageBig.activeInHierarchy)
        //{
        //    TogglePromptSize();
        //}
    }

    //public void MaximizePrompt()
    //{
    //    photoAlbumContainer.SetActive(false);
    //    promptImageBig.SetActive(true);
    //    PromptPreviewManager.Instance.HidePromptPreview();
    //    Color promptCol = promptImageBig.GetComponent<Image>().color;
    //    promptCol.a = 0.5f;
    //    if (isPromptSolved)
    //    {
    //        promptCol.r = 1;
    //        promptCol.g = 1;
    //        promptCol.b = 1;
    //    }
    //    else
    //    {
    //        promptCol.r = 0.5f;
    //        promptCol.g = 0.5f;
    //        promptCol.b = 0.9f;
    //    }
    //    promptImageBig.GetComponent<Image>().color = promptCol;
    //}

    //public void MinimizePrompt()
    //{
    //    promptImageBig.SetActive(false);
    //    //photoAlbumContainer.SetActive(true);
    //    PromptPreviewManager.Instance.ShowPromptPreview();
    //}

    //private void TogglePromptSize()
    //{
    //    if (photoAlbumContainer.activeInHierarchy && !promptImageBig.activeInHierarchy)
    //    {
    //        photoAlbumContainer.SetActive(false);
    //        promptImageBig.SetActive(true);
    //        PromptPreviewManager.Instance.HidePromptPreview();
    //        Color promptCol = promptImageBig.GetComponent<Image>().color;
    //        promptCol.a = 0.5f;
    //        if (isPromptSolved)
    //        {
    //            promptCol.r = 1;
    //            promptCol.g = 1;
    //            promptCol.b = 1;
    //        }
    //        else
    //        {
    //            promptCol.r = 0.5f;
    //            promptCol.g = 0.5f;
    //            promptCol.b = 0.9f;
    //        }
    //        promptImageBig.GetComponent<Image>().color = promptCol;
    //    }
    //    else if (promptImageBig.activeInHierarchy && !photoAlbumContainer.activeInHierarchy)
    //    {
    //        promptImageBig.SetActive(false);
    //        photoAlbumContainer.SetActive(true);
    //        PromptPreviewManager.Instance.ShowPromptPreview();
    //    }

    //}
}