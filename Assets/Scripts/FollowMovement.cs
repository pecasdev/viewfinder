using UnityEngine;

public class FollowMovement : MonoBehaviour
{

    public GameObject following;
    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, following.transform.position, 1);
    }
}