using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldPhotoController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip captionAppearSound;

    // New Photo UI
    [SerializeField] private UnityEngine.UI.Image _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private Animator _photoFadeAnimator;
    private Animator _photoFrameAnimator;
    private bool canTakePhoto = true;


    private static HeldPhotoController instance = null;
    public static HeldPhotoController Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("HeldPhotoController is NULL");
            }
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    void Start()
    {
        _photoFrameAnimator = _photoFrame.GetComponent<Animator>();

    }

    public IEnumerator DisplayPhoto()
    {
        CanTakePhoto = false;
        _photoFrame.SetActive(true);
        _photoFrameAnimator.Play("ViewPhoto");
        yield return new WaitForSeconds(4f);
        _photoFrame.SetActive(false);
        CanTakePhoto = true;
    }

    public IEnumerator PhotoMatchesPrompt()
    {
        CanTakePhoto = false;
        _photoFrame.SetActive(true);
        _photoFrameAnimator.Play("PhotoMatchesPrompt");
        _photoFadeAnimator.Play("PhotoFadeIn");
        yield return new WaitForSeconds(1f);
        PhotoAlbumManager.Instance.OpenPhotoAlbum();
        PhotoAlbumManager.Instance.RevealCaption();
        audioSource.clip = captionAppearSound;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        _photoFrame.SetActive(false);
        PhotoAlbumManager.Instance.UpdatePhotoAlbum();
        CanTakePhoto = true;

    }

    public void SetHeldPhotoImage(Sprite sprite)
    {
        _photoDisplayArea.sprite = sprite;
    }

    public Sprite GetHeldPhotoImage()
    {
        return _photoDisplayArea.sprite;
    }
    public bool CanTakePhoto
    {
        get { return canTakePhoto; }
        private set { canTakePhoto = value; }
    }
}
