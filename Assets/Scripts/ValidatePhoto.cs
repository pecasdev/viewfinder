using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValidatePhoto : MonoBehaviour
{

    public const float ACCEPT_POS_DEVIATION = 5;
    public const float ACCEPT_ANGLE_DEVIATION = 10;
    private new GameObject camera;



    void Start()
    {
        camera = GameObject.FindGameObjectsWithTag("1st person camera")[0];
    }

    public bool IsAttemptAcceptable(Vector3 attemptPos, Vector3 attemptAngle)
    {
        float posDeviation = (attemptPos - GameManager.Instance.GetPositionSolution()).magnitude;
        float angDeviation = angleDeviation(attemptAngle, GameManager.Instance.GetAngleSolution()).magnitude;
        return (posDeviation <= ACCEPT_POS_DEVIATION) && (angDeviation <= ACCEPT_ANGLE_DEVIATION);
    }

    private Vector3 angleDeviation(Vector3 va, Vector3 vb)
    {
        return new Vector3(
          Mathf.DeltaAngle(va.x, vb.x),
          Mathf.DeltaAngle(va.y, vb.y),
          Mathf.DeltaAngle(va.z, vb.z)
        );
    }

    // Update is called once per frame

    public virtual bool validatePhoto()
    {
        Vector3 attemptPos = camera.transform.position;
        Vector3 attemptAngle = camera.transform.eulerAngles;

        Debug.Log(string.Format("POS: {0} ANG: {1}", attemptPos, attemptAngle));

        if (IsAttemptAcceptable(attemptPos, attemptAngle))
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
    void Update()
    {
      
    }
}