using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movep : MonoBehaviour
{
    public Camera mainCamera; // Make sure to assign your main camera in the Inspector
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public float moveSpeed = 5.0f;  // Player movement speed
    public float turnSpeed = 20.0f;  // Player turning speed
    public float jumpForce = 5.0f;

    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // get keyboard input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool playerJumped = Input.GetButton("Jump");

        // Calculate direction based on camera orientation
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;  // Zero out the y-component
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        m_Movement = cameraForward * vertical + cameraRight * horizontal;

        // apply movement to the Rigidbody
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * moveSpeed * Time.fixedDeltaTime);

        // make the player turn, but only if moving to avoid unnecessary rotations
        if (m_Movement != Vector3.zero)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.fixedDeltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            m_Rigidbody.MoveRotation(m_Rotation);
        }

        if (playerJumped)
        {
            m_Rigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
            playerJumped = false;
        }

    }
}
