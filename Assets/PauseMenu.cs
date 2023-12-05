using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, optionsMenu;
    //public static bool gameIsPaused = false;
    public GameObject pauseFirstButton, optionsFirstButton;
    private bool b_button_pressed;
    private bool start_button_pressed;
    private GameManager.GameState prev_State;

    string Xbox_Start_Button;

    public enum UIAction
    {
        On,
        Off,
        Toggle
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (UnityEngine.Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                Xbox_Start_Button = "Xbox_Start_Button";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                Xbox_Start_Button = "Xbox_Start_Button Mac";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }
        float start_button_val = Input.GetAxis(Xbox_Start_Button);
        if (start_button_val == 0)
        {
            start_button_pressed = false;
        }
        if (Input.GetAxis(Xbox_Start_Button) == 1 && !start_button_pressed)
        {
            start_button_pressed = true;
            if (GameManager.Instance.currentGameSate == GameManager.GameState.PausedMenu)
            {
                CloseOptionsMenu();
            }        
            TogglePause();
        }
        else if (GameManager.Instance.currentGameSate == GameManager.GameState.PausedMenu)
        {

            float b_button_val = Input.GetAxis("Xbox_B_Button");
            if (b_button_val == 0)
            {
                b_button_pressed = false;
            }

            if (Input.GetAxis("Xbox_B_Button") == 1 && !b_button_pressed)
            {
                b_button_pressed = true;
                if (pauseMenu.activeInHierarchy)
                {
                    TogglePause();
                }
                else
                {
                    CloseOptionsMenu();
                }
            }
        }
    }

    private void TogglePause()
    {
        if (GameManager.Instance.currentGameSate != GameManager.GameState.PausedMenu)
        {
            prev_State = GameManager.Instance.currentGameSate;
            GameManager.Instance.currentGameSate = GameManager.GameState.PausedMenu;
        }
        else
        {
            GameManager.Instance.currentGameSate = prev_State;
        }
        
        //gameIsPaused = !gameIsPaused;
        // TODO: Add this if/else logic in other functions
        if (GameManager.Instance.currentGameSate == GameManager.GameState.PausedMenu)
        {
            ToggleUI(pauseMenu, pauseFirstButton);
        }
        else
        {
            ToggleUI(pauseMenu, uiAction: UIAction.Off);
            ToggleUI(optionsMenu, uiAction: UIAction.Off);

        }
    }

    private void ToggleUI(GameObject uiObj, GameObject selectBtn = null, UIAction uiAction = UIAction.Toggle)
    {
        if (uiAction == UIAction.Toggle)
        {
            uiObj.SetActive(!uiObj.activeInHierarchy);
        }

        else if (uiAction == UIAction.On)
        {
            uiObj.SetActive(true);
        }

        else if (uiAction == UIAction.Off)
        {
            uiObj.SetActive(false);
        }

        if (selectBtn != null)
        {
            SelectGameObject(selectBtn);
        }
    }
    private void SelectGameObject(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    // For use in the editor
    // Pause Menu Button Functions
    public void ResumeGame()
    {
        TogglePause();
    }

    public void RestartLevel()
    {
        GameManager.Instance.currentGameSate = GameManager.GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenOptionsMenu()
    {
        ToggleUI(optionsMenu, optionsFirstButton, UIAction.On);
        ToggleUI(pauseMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Options Menu Button Functions
    public void CloseOptionsMenu()
    {
        ToggleUI(optionsMenu, uiAction: UIAction.Off);
        ToggleUI(pauseMenu, pauseFirstButton);
    }
}
