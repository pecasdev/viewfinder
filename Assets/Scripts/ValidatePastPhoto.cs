using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValidatePastPhoto : ValidatePhoto
{
    private new GameObject camera;

    void Start()
    {
        camera = GameObject.FindGameObjectsWithTag("1st person camera")[0];
    }

    

    private bool _additionalValidation()
    {
        // later on will use this to check if objects are in correct spots etc.
        // based on the GameManager.currentStage
        return true;
    }

    public override bool validatePhoto() 
    {
        Vector3 attemptPos = camera.transform.position;
        Vector3 attemptAngle = camera.transform.eulerAngles;

        Debug.Log(string.Format("POS: {0} ANG: {1}", attemptPos, attemptAngle));

        if (base.IsAttemptAcceptable(attemptPos, attemptAngle) && _additionalValidation())
        {
            Debug.Log("REWARD");
            GameManager.Instance.UpdateStage();
            return true;
        }
        else
        {
            Debug.Log("NO REWARD");
            return false;
        }
    }

}