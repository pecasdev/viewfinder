using UnityEngine;

public class FollowMovement : MonoBehaviour
{

    public GameObject following;
    
    void LateUpdate()
    {
        //int distanceInfront = 50;
        Vector3 lookVector = (following.transform.position - gameObject.transform.position);
        gameObject.transform.localRotation = Quaternion.LookRotation(lookVector);
    }
}