using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFTUE : MonoBehaviour
{
    int stepIdx;
    [SerializeField] private GameObject[] steps;
    private string RightStickHorizontal;
    private string RightStickVertical;
    bool leftStickRegistered;
    bool rightStickRegistered;
    private string LeftStickHorizontal;
    private string LeftStickVertical;

    // Start is called before the first frame update
    void Start()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                LeftStickHorizontal = "LeftStickHorizontal Windows";
                LeftStickVertical = "LeftStickVertical Windows";
                RightStickHorizontal = "RightStickHorizontal Windows";
                RightStickVertical = "RightStickVertical Windows";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                RightStickHorizontal = "RightStickHorizontal Mac";
                RightStickVertical = "RightStickVertical Mac";
                break;
        }
        stepIdx = 0;
        GameManager.Instance.currentGameSate = GameManager.GameState.MovementFTUE;
        steps[stepIdx].SetActive(true);
        leftStickRegistered = false;
        rightStickRegistered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (stepIdx == 0)
        {
            if (Input.GetAxis(LeftStickHorizontal) != 0 || Input.GetAxis(LeftStickVertical) != 0) // check for movement
            {
                if (!leftStickRegistered)
                {
                    leftStickRegistered = true;
                    StartCoroutine(NextStep());
                }
            }
        }
        if (stepIdx == 1)
        {
            if (Input.GetAxis(RightStickHorizontal) != 0 || Input.GetAxis(RightStickVertical) != 0) // check for looking around
            {
                if (!rightStickRegistered)
                {
                    rightStickRegistered = true;
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
            PlayerPrefs.SetInt("movementFTUEComplete", 1);
            yield return new WaitForSeconds(3f);
            steps[stepIdx].SetActive(false);
            FTUEManager.Instance.CheckFTUE();
        }
    }
}
