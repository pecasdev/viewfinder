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
    public float pastCameraDisplacement = 50f;
    string RightStickHorizontal;
    string RightStickVertical;
    bool dpad_pressed = false;


    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        switch (UnityEngine.Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                RightStickHorizontal = "RightStickHorizontal Windows";
                RightStickVertical = "RightStickVertical Windows";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                RightStickHorizontal = "RightStickHorizontal Mac";
                RightStickVertical = "RightStickVertical Mac";
                break;
        }
    }

    void Update()
    {
        float mouseX;
        float mouseY;

        if (Input.GetAxis(RightStickHorizontal) != 0.0f || Input.GetAxis(RightStickVertical) != 0)
        {

            mouseX = Input.GetAxis(RightStickHorizontal);
            mouseY = Input.GetAxis(RightStickVertical);
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

        /*if (playerInPast)
        {
            pastCameraDisplacement = -pastCameraDisplacement;
        }*/

        if (isPastCamera)
        {
            Vector3 playerPosition = player.position;

            Vector3 newPosition = new Vector3(player.position.x, playerPosition.y - pastCameraDisplacement + 0.6f, player.position.z);

            transform.position = newPosition;
        }
        else
        {
            transform.position = new Vector3(player.position.x, player.position.y + 0.6f, player.position.z); ;
        }


        // navigate photo album
        float dpad_v_Value = Input.GetAxis("DPAD_v Windows");
        float dpad_h_Value = Input.GetAxis("DPAD_h Windows");
        if (dpad_v_Value == 0 && dpad_h_Value == 0)
        {
            dpad_pressed = false;
        }

        if (Input.GetAxis("DPAD_h Windows") == 1 && !dpad_pressed)
        {
            dpad_pressed = true;
            PhotoAlbumManager.Instance.ChangeStage(GameManager.StageOrder.Previous);
        }
        else if (Input.GetAxis("DPAD_h Windows") == -1 && !dpad_pressed)
        {
            dpad_pressed = true;
            PhotoAlbumManager.Instance.ChangeStage(GameManager.StageOrder.Next);
        }
        else if (Input.GetAxis("DPAD_v Windows") == -1 && !dpad_pressed)
        {
            dpad_pressed = true;
            PhotoAlbumManager.Instance.OpenPhotoAlbum();
        }
        else if (Input.GetAxis("DPAD_v Windows") == 1 && !dpad_pressed)
        {
            dpad_pressed = true;
            PhotoAlbumManager.Instance.ClosePhotoAlbum();
        }
        //bool switchView = Input.GetKeyDown(KeyCode.Q) || (dpad_up_Value == 1 && !dpad_pressed);
        bool switchView = Input.GetKeyDown(KeyCode.Q);
        if (switchView)
        {
            Vector3 tempPlayer = player.position;
            if (isPastCamera)
            {
                Vector3 futurePosition = new Vector3(transform.position.x, transform.position.y + pastCameraDisplacement, transform.position.z);
                transform.position = futurePosition;
            }
            else
            {
                Vector3 pastPosition = new Vector3(player.position.x, player.position.y - pastCameraDisplacement, player.position.z);
                player.position = pastPosition;
            }
            pastCameraDisplacement = -pastCameraDisplacement;

        }


    }
}