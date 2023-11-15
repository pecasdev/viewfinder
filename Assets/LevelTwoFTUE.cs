using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoFTUE : MonoBehaviour
{
    int stepIdx;
    [SerializeField] private GameObject[] steps;
    private string x_btn;
    private bool x_btn_Registered;

    // Start is called before the first frame update
    void Start()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                x_btn = "Xbox_X_Button";
                break;
        }
        stepIdx = 0;
        GameManager.Instance.currentGameSate = GameManager.GameState.Level2MechanicFTUE;
        steps[stepIdx].SetActive(true);
        x_btn_Registered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (stepIdx == 0)
        {
            if (Input.GetAxis(x_btn) == 1 || Input.GetKeyDown(KeyCode.E)) // open album

            {
                if (!x_btn_Registered)
                {
                    x_btn_Registered = true;
                    StartCoroutine(NextStep());
                }
            }
        }
    }

    IEnumerator NextStep()
    {
        yield return new WaitForSeconds(3f);
        steps[stepIdx].SetActive(false);
        stepIdx++;
        steps[stepIdx].SetActive(true);
        if (stepIdx == steps.Length - 1)
        {
            PlayerPrefs.SetInt("level2MechanicFTUE", 1);
            yield return new WaitForSeconds(3f);
            steps[stepIdx].SetActive(false);
            FTUEManager.Instance.CheckFTUE();
        }
    }
}