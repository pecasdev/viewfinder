using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFTUE : MonoBehaviour
{
    int stepIdx;
    [SerializeField] private GameObject[] steps;
    private string leftTrigger;
    private string rightTrigger;
    private bool leftTriggerRegistered;
    private bool rightTriggerRegistered;

    // Start is called before the first frame update
    void Start()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                leftTrigger = "Left Trigger Windows";
                rightTrigger = "Right Trigger Windows";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                leftTrigger = "Left Trigger Mac";
                rightTrigger = "Right Trigger Mac";
                break;
        }
        stepIdx = 0;
        GameManager.Instance.currentGameSate = GameManager.GameState.CameraFTUE;
        steps[stepIdx].SetActive(true);
        leftTriggerRegistered = false;
        rightTriggerRegistered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (stepIdx == 0)
        {
            if (Input.GetAxis(leftTrigger) == -1 || Input.GetKeyDown(KeyCode.Mouse1)) // check for looking into camera
            {
                if (!leftTriggerRegistered)
                {
                    leftTriggerRegistered = true;
                    StartCoroutine(NextStep());
                }
            }
        }
        if (stepIdx == 1)
        {
            if (Input.GetAxis(rightTrigger) == -1 || Input.GetKeyDown(KeyCode.Mouse0)) // check for looking around
            {
                if (!rightTriggerRegistered)
                {
                    rightTriggerRegistered = true;
                    StartCoroutine(NextStep());
                }
            }
        }
    }

    IEnumerator NextStep()
    {
        yield return new WaitForSeconds(1.5f);
        steps[stepIdx].SetActive(false);
        stepIdx++;
        steps[stepIdx].SetActive(true);
        if (stepIdx == steps.Length - 1)
        {
            PlayerPrefs.SetInt("cameraFTUEComplete", 1);
            yield return new WaitForSeconds(3f);
            steps[stepIdx].SetActive(false);
            GameManager.Instance.currentGameSate = GameManager.GameState.Playing;
            FTUEManager.Instance.CheckFTUE();
        }
    }
}
