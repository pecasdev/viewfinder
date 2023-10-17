using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static System.Net.Mime.MediaTypeNames;

public class ModelCameraController : MonoBehaviour
{
    public AudioClip[] shutterSounds;
    private AudioSource audioSource;

    string leftTrigger;
    string rightTrigger;
    string leftButton;
    string rightButton;

    [SerializeField] private GameObject _camInterface, _aperture;
    //[SerializeField] private PostProcessVolume _volume;
    [SerializeField] private ValidatePhoto _photoValidator;
    //private Vignette _vignetting;
    private Animator _cameraAnims;
    private bool _canTakePhoto, _printing;
    public bool CanTakePhoto
    {
        get { return _canTakePhoto; }
        set { _canTakePhoto = value; }
    }
    private void Start()
    {
        CanTakePhoto = false;
        //_volume.profile.TryGetSettings(out _vignetting);
        _cameraAnims = GetComponent<Animator>();
        switch (UnityEngine.Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                rightButton = "Right Button Windows";
                leftButton = "Left Button Windows";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                rightButton = "Right Button Mac";
                leftButton = "Left Button Mac";
                break;
        }

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = Input.GetAxis(leftTrigger) + Input.GetAxis(rightTrigger);
        if (Input.GetMouseButton(1) || (triggerValue < -0.1f))
        {
            _cameraAnims.SetBool("TakingPhoto", true);
        }
        else if (_cameraAnims.GetBool("TakingPhoto") && !_printing)
        {
            _cameraAnims.SetBool("TakingPhoto", false);
            CanTakePhoto = false;
            SetCameraState(false);
        }
    }

    private void playCameraShutterSound()
    {
        int index = Random.Range(0, shutterSounds.Length);
        audioSource.clip = shutterSounds[index];
        audioSource.Play();
    }

    public void OnCameraActive()
    {
        SetCameraState(true);
        CanTakePhoto = true;
    }

    public void TakePhoto()
    {
        StartCoroutine(ShowAperture());
        _photoValidator.validatePhoto();
        playCameraShutterSound();  
    }

    public IEnumerator ShowAperture()
    {
        _printing = true;
        _aperture.SetActive(true);
        SetCameraState(true);
        yield return new WaitForSeconds(0.25f);
        _aperture.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SetCameraState(false);
        _cameraAnims.SetTrigger("TakePhoto");
    }

    public void OnPrint()
    {
        _printing = false;
    }

    public void SetCameraState(bool state)
    {
        _camInterface.SetActive(state);
        //_vignetting.enabled.value = state;
    }
}
