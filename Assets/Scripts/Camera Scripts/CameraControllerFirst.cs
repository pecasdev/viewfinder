using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraControllerFirst : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 90f;
    Vector2 rotation = Vector2.zero;
    public bool isPastCamera;
    public float pastCameraDisplacement = 15f;


    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX;
        float mouseY;
        
        if(Input.GetAxis("RightStickHorizontal") != 0.0f || Input.GetAxis("RightStickVertical") != 0)
        {
            
            mouseX = Input.GetAxis("RightStickHorizontal");
            mouseY = Input.GetAxis("RightStickVertical");
            // mouseX = Input.GetAxis("Mouse X");
            // mouseY = Input.GetAxis("Mouse Y");
        }
        else
        {
            // mouseX = Input.GetAxis("RightStickHorizontal");
            // mouseY = Input.GetAxis("RightStickVertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        mouseX = mouseX * sensitivity;
        mouseY = mouseY * sensitivity;
        // float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        // float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        rotation.x += mouseX * sensitivity;
        rotation.y += mouseY * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);

        if (isPastCamera)
        {
            // Get the player's position
            Vector3 playerPosition = player.position;

            // Create a new position with the y-coordinate 15 units less than the player's y-coordinate
            Vector3 newPosition = new Vector3(player.position.x, playerPosition.y - pastCameraDisplacement + 0.6f, player.position.z);

            // Update the object's position
            transform.position = newPosition;
        }
        else
        {
            // Ensure camera holder position matches the player's position
            transform.position = new Vector3(player.position.x, player.position.y + 0.6f, player.position.z); ;
        }
        
    }
}

