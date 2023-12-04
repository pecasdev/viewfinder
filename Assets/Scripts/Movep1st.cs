using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Movep1st : MonoBehaviour
{
    Camera mainCamera;
    public Camera firstPersonCamera;
    public Camera thirdPersonCamera;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public float moveSpeed = 5.0f;  // Player movement speed
    public float turnSpeed = 20.0f;  // Player turning speed
    public float jumpForce = 5.0f;
    public bool firstPersonViewOn = true;
    public LayerMask groundLayer;
    public bool sprint_pressed = false;
    private bool lock_sprint_button = false;

    bool playerGrounded = true;

    Rigidbody m_Rigidbody;

    public AudioSource movementAudioSource;
    public AudioClip movementAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        mainCamera = firstPersonCamera;
        thirdPersonCamera.enabled = false;

        // Set up the audio source
        if (movementAudioSource == null)
        {
            movementAudioSource = gameObject.AddComponent<AudioSource>();
        }
        movementAudioSource.clip = movementAudioClip;
        movementAudioSource.loop = true;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.currentGameSate == GameManager.GameState.PausedMenu || (GameManager.Instance.currentGameSate != GameManager.GameState.MovementFTUE && GameManager.Instance.currentGameSate != GameManager.GameState.Level2MechanicFTUE && GameManager.Instance.currentGameSate != GameManager.GameState.Level3MechanicFTUE && GameManager.Instance.currentGameSate != GameManager.GameState.Playing)) return;
        // get keyboard input
        moveSpeed = 5.0f;
        
        if (Input.GetAxis("Left_Stick_Button") <= 0){
            lock_sprint_button = false;
        }
        if (Input.GetAxis("Left_Stick_Button") > 0 && !lock_sprint_button)
        {
            sprint_pressed = !sprint_pressed;
            lock_sprint_button = true;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || sprint_pressed)
        {
            moveSpeed = 7.0f;  // Increase speed by factor of 2
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //bool playerJumped = Input.GetButton("Jump");

        // Calculate direction based on camera orientation
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0;  // Zero out the y-component
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        m_Movement = cameraForward * vertical + cameraRight * horizontal;

        // Check if the player is moving
        if (m_Movement != Vector3.zero)
        {
            // If not already playing, start the sound
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.Play();
            }
        }
        else
        {
            // If no movement and the sound is playing, stop it
            if (movementAudioSource.isPlaying)
            {
                movementAudioSource.Stop();
            }
        }

        // apply movement to the Rigidbody
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * moveSpeed * Time.fixedDeltaTime);

        // make the player turn, but only if moving to avoid unnecessary rotations
        if (m_Movement != Vector3.zero)
        {
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.fixedDeltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            m_Rigidbody.MoveRotation(m_Rotation);
        }

        //if (playerJumped && playerGrounded)
        //{
        //    m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //    playerGrounded = false;
        //}

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            playerGrounded = true;
        }
    }
}
