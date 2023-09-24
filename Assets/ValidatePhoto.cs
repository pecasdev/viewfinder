using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValidatePhoto : MonoBehaviour
{
    Vector3 SOLUTION_POS;
    Vector3 SOLUTION_ANGLE;
    // Start is called before the first frame update

    public float ACCEPT_POS_DEVIATION = 5;
    public float ACCEPT_ANGLE_DEVIATION = 10;

    GameObject capsule;
    new GameObject camera;

    GameObject level;


    void Start()
    {
        capsule = GameObject.FindGameObjectsWithTag("Player")[0];
        camera = GameObject.FindGameObjectsWithTag("MainCamera")[0];

        SOLUTION_POS = new Vector3(-6.33f, 2.5f, 5.82f);
        SOLUTION_ANGLE = new Vector3(33.2f, 308f, 0f);
    }

    bool isAttemptAcceptable(Vector3 attempt, Vector3 solution, bool isAngle)
    {
        if (isAngle)
        {
            Debug.Log(string.Format("{0} {1}", attempt, solution));
            //Debug.Log(string.Format("{1} {2}", new Vector2(attempt.x, attempt.y), new Vector2(solution.x, solution.y)));
            float angdev = angleDeviation(attempt, solution).magnitude;//Vector2.Angle(attempt, solution);
            Debug.Log("ANGDEV2: " + angdev);
            return angdev <= ACCEPT_ANGLE_DEVIATION;
        }
        else
        {
            float posDeviation = (attempt - solution).magnitude;
            Debug.Log("POSDEV: " + posDeviation);
            return posDeviation <= ACCEPT_POS_DEVIATION;
        }
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
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Vector3 attemptPos = capsule.transform.position;
            Vector3 attemptAngle = camera.transform.eulerAngles;

            Debug.Log(string.Format("POS: {0} ANG: {1}", attemptPos, attemptAngle));

            bool attemptPosAccepted = isAttemptAcceptable(attemptPos, SOLUTION_POS, false);
            bool attemptAngleAccepted = isAttemptAcceptable(attemptAngle, SOLUTION_ANGLE, true);

            if (attemptPosAccepted && attemptAngleAccepted)
            {
                Debug.Log("REWARD");
                GameManager.Instance.LevelText.GetComponent<TextMeshProUGUI>().text = "Success!";
                GameManager.Instance.TutorialText.SetActive(false);
            }
            else
            {
                Debug.Log("NO REWARD");
            }
        }
    }
}