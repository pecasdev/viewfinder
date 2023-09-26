using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float rotationSpeed = 5f;
    private float xRotation = 0f; // To keep track of vertical rotation
    private float cameraDistance;  // Distance of the camera from the pivot.

    void Start()
    {
        // Calculate initial camera distance based on its position relative to CameraHolder
        cameraDistance = Vector3.Distance(transform.position, transform.GetChild(0).position);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Rotate the CameraHolder in Y axis (Horizontal rotation)
        transform.Rotate(Vector3.up * mouseX);

        // Adjust the camera's vertical rotation around the CameraHolder
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Transform camera = transform.GetChild(0);
        Vector3 cameraOffset = new Vector3(0, 0, -cameraDistance);
        cameraOffset = Quaternion.Euler(xRotation, 0, 0) * cameraOffset;
        camera.localPosition = cameraOffset;
        camera.LookAt(transform);


        // Ensure camera holder position matches the player's position
        transform.position = player.position;
    }

}
