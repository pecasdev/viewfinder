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


    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        rotation.x += mouseX * sensitivity;
        rotation.y += mouseY * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);

        // Ensure camera holder position matches the player's position
        transform.position = player.position;
    }
}

