using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

public class ScreenshotController : MonoBehaviour
{
    [SerializeField] private int _resWidth = 1920;
    [SerializeField] private int _resHeight = 1080;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _creationPoint;
    [SerializeField] private ModelCameraController _modelCameraController;
    private bool _usePastCamera = false; // Bool variable to switch between cameras.
    [SerializeField] private bool _teleport = false; // Bool variable to switch between cameras.
    // Define an enumeration for the three choices
    public enum Choices
    {
        Present,
        Past
    }

    // Use SerializeField to expose the private field to the Unity Editor
    [SerializeField]
    private Choices CameraAbility;

    private SpriteRenderer _spriteRenderer;
    private GameObject _createdPhoto;
    private Camera pastCamera;
    Plane[] planes;

    private void Start()
    {
        pastCamera = GameObject.FindGameObjectWithTag("Past Camera").GetComponent<Camera>();
        if (pastCamera == null)
        {
            UnityEngine.Debug.LogError("No camera found with the tag Past camera");
        }
    }

    public void LateUpdate()
    {
        switch (CameraAbility)
        {
            case Choices.Past:
                _usePastCamera = true;
                break;
            case Choices.Present:
                _usePastCamera = false;
                break;
        }
        if (_modelCameraController.CanTakePhoto && (Input.GetMouseButtonDown(0) || Input.GetKeyDown("p")))
        {
            _modelCameraController.SetCameraState(false);
            RenderTexture rt = new RenderTexture(_resWidth, _resHeight, 24);
            Camera cameraToUse = _usePastCamera ? pastCamera : GameObject.FindWithTag("1st person camera").GetComponent<Camera>(); // Choose the camera based on the _usePastCamera flag.

            if (cameraToUse == null) // Check if the camera is null.
            {
                UnityEngine.Debug.LogError("No camera available to take screenshot");
                return; // Exit the method if no camera is available.
            }

            // If using the past camera, enable it temporarily
            if (_usePastCamera)
            {
                pastCamera.enabled = true;
            }

            // check for teleportable objects
            if (_teleport)
            {
                RefreshAllTeleportables();
                TeleportObjects(pastCamera);
                TeleportObjects(Camera.main);
            }

            ////////////////////////////////////////

            cameraToUse.targetTexture = rt;
            Texture2D screenShot = new Texture2D(_resWidth, _resHeight, TextureFormat.RGB24, false);
            cameraToUse.Render();
            cameraToUse.targetTexture = null;

            // Disable the past camera
            if (_usePastCamera)
            {
                pastCamera.enabled = false;
            }

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, _resWidth, _resHeight), 0, 0);
            RenderTexture.active = null;
            screenShot.Apply();
            Destroy(rt);

            Sprite targetSp = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), Vector2.one * 0.5f, 1000f);
            if (_createdPhoto != null)
            {
                Destroy(_createdPhoto);
            }
            _createdPhoto = Instantiate(_cardPrefab, _creationPoint);
            _createdPhoto.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _spriteRenderer = _createdPhoto.transform.GetChild(0).GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = targetSp;
            _createdPhoto.transform.localScale = new Vector3(0.64f, 0.37f, 0.08f);
            _modelCameraController.CanTakePhoto = false;
            _modelCameraController.SetCameraState(true);
            _modelCameraController.TakePhoto();
        }
    }

    void TeleportObjects(Camera cameraToUse)
    {
        // check for teleportable objects
        Teleportable[] Teleportables = FindObjectsOfType<Teleportable>().Where(p => p.isActiveAndEnabled).ToArray();

        foreach (var teleportable in Teleportables)
        {
            // Assuming the teleportable object has a Renderer component to get the bounds
            Renderer renderer = teleportable.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Check if the bounding box of the teleportable object is in the camera's frustum
                if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cameraToUse), renderer.bounds))
                {
                    // UnityEngine.Debug.Log("Object in Camera Frustum: " + teleportable.gameObject.name + cameraToUse.name);
                    // Set isInThePresent to false and decrease the y value by 15
                    if (teleportable.getSwitchedAlready())
                    {
                        continue;
                    } else
                    {
                        teleportable.setSwitchedAlready(true);
                    }

                    if (teleportable.IsInThePresent)
                    {
                        teleportable.gameObject.transform.position -= new Vector3(0, 15, 0);
                    }
                    else
                    {
                        teleportable.gameObject.transform.position += new Vector3(0, 15, 0);
                    }
                    teleportable.IsInThePresent = !teleportable.IsInThePresent;
                    

                }
            }
        }

        ////////////////////////////////////////
    }

    void RefreshAllTeleportables()
    {
        Teleportable[] Teleportables = FindObjectsOfType<Teleportable>().Where(p => p.isActiveAndEnabled).ToArray();

        foreach (var teleportable in Teleportables)
        {
            teleportable.setSwitchedAlready(false);
        }
    }


}
