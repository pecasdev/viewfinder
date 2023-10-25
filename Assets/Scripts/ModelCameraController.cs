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

    // New Photo UI
    [SerializeField] private UnityEngine.UI.Image _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private Animator _photoFadeAnimator;
    private Animator _photoFrameAnimator;


    private void Start()
    {
        CanTakePhoto = false;
        //_volume.profile.TryGetSettings(out _vignetting);
        _cameraAnims = GetComponent<Animator>();
        _photoFrameAnimator = _photoFrame.GetComponent<Animator>();

        switch (UnityEngine.Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                leftTrigger = "Left Trigger Windows";
                rightTrigger = "Right Trigger Windows";
                rightButton = "Right Button Windows";
                leftButton = "Left Button Windows";
                break;

            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
                leftTrigger = "Left Trigger Mac";
                rightTrigger = "Right Trigger Mac";
                rightButton = "Right Button Mac";
                leftButton = "Left Button Mac";
                break;
        }

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused) return;
        float triggerValue = Input.GetAxis(leftTrigger) + Input.GetAxis(rightTrigger);
        //float triggerValue = Input.GetAxis(leftTrigger);
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
        bool isValid = _photoValidator.validatePhoto();
        if (isValid)
        {
            StartCoroutine(PhotoMatchesPrompt());
        } else
        {
            StartCoroutine(DisplayPhoto());
        }
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

    IEnumerator DisplayPhoto()
    {
        _photoFrame.SetActive(true);
        _photoFrameAnimator.Play("ViewPhoto");
        yield return new WaitForSeconds(4f);
        _photoFrame.SetActive(false);
    }

    IEnumerator PhotoMatchesPrompt()
    {
        _photoFrame.SetActive(true);
        _photoFrameAnimator.Play("PhotoMatchesPrompt");
        _photoFadeAnimator.Play("PhotoFadeIn");
        yield return new WaitForSeconds(1f);
        PhotoAlbumManager.Instance.OpenPhotoAlbum();
        yield return new WaitForSeconds(1f);
        _photoFrame.SetActive(false);
    }

}
