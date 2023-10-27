using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldPhotoController : MonoBehaviour
{
    // New Photo UI
    [SerializeField] private UnityEngine.UI.Image _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private Animator _photoFadeAnimator;
    private Animator _photoFrameAnimator;

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
    }
    void Start()
    {
        _photoFrameAnimator = _photoFrame.GetComponent<Animator>();

    }

    public IEnumerator DisplayPhoto()
    {
        _photoFrame.SetActive(true);
        _photoFrameAnimator.Play("ViewPhoto");
        yield return new WaitForSeconds(4f);
        _photoFrame.SetActive(false);
    }

    public IEnumerator PhotoMatchesPrompt()
    {
        _photoFrame.SetActive(true);
        _photoFrameAnimator.Play("PhotoMatchesPrompt");
        _photoFadeAnimator.Play("PhotoFadeIn");
        yield return new WaitForSeconds(1f);
        PhotoAlbumManager.Instance.OpenPhotoAlbum();
        PhotoAlbumManager.Instance.RevealCaption();
        yield return new WaitForSeconds(1f);
        _photoFrame.SetActive(false);
        yield return new WaitForSeconds(2f);
        PhotoAlbumManager.Instance.UpdatePhotoAlbum();
    }

    public void SetHeldPhotoImage(Sprite sprite)
    {
        _photoDisplayArea.sprite = sprite;
    }

}
