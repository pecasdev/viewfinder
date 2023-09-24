using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class move : MonoBehaviour
{
    public Camera mainCamera;
    float speed = 20f;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forwardMovement = mainCamera.transform.forward * vertical;
        Vector3 rightMovement = mainCamera.transform.right * horizontal;

        // Zero out the y-component to ensure the character doesn't move vertically
        forwardMovement.y = 0f;
        rightMovement.y = 0f;

        // Normalize the vectors if the input is non-zero to maintain consistent speed in diagonal directions
        Vector3 finalMovement = forwardMovement + rightMovement;
        if (finalMovement.magnitude > 1f)
        {
            finalMovement = finalMovement.normalized;
        }

        // Multiply the finalMovement by your character's speed, and apply it to your character's position or Rigidbody.
        finalMovement *= speed * Time.deltaTime;

        // Example: If you're using a Rigidbody for movement
        // rb.MovePosition(rb.position + finalMovement);

        // If you're simply updating the transform's position
        transform.position += finalMovement;
    }
}
