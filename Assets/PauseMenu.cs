using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, optionsMenu;
    public static bool gameIsPaused = false;
    public GameObject pauseFirstButton, optionsFirstButton;
    private bool b_button_pressed;
    private bool start_button_pressed;

    public enum UIAction
    {
        On,
        Off,
        Toggle
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglePause();
        }
        float start_button_val = Input.GetAxis("Xbox_Start_Button");
        if (start_button_val == 0)
        {
            start_button_pressed = false;
        }
        if (Input.GetAxis("Xbox_Start_Button") == 1 && !start_button_pressed)
        {
            start_button_pressed = true;
            if (gameIsPaused)
            {
                CloseOptionsMenu();
            }        
            TogglePause();
        }
        else if (gameIsPaused)
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
        gameIsPaused = !gameIsPaused;
        // TODO: Add this if/else logic in other functions
        if (gameIsPaused)
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
        gameIsPaused = false;
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
