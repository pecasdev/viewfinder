using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class PlantListWrapper
{
    public List<PlantToWater> plants;
}


public class ValidatePlantPhoto : ValidatePhoto
{
    private GameObject _camera;
    [SerializeField] private List<PlantListWrapper> _promptToPlants;

    // New Photo UI
    [SerializeField] private UnityEngine.UI.Image _photoDisplayArea;
    [SerializeField] private GameObject _photoFrame;
    [SerializeField] private Animator _photoFadeAnimator;
    private Animator _photoFrameAnimator;

    void Start()
    {
        _camera = GameObject.FindGameObjectsWithTag("1st person camera")[0];
        _photoFrameAnimator = _photoFrame.GetComponent<Animator>();


    }



    private bool _additionalValidation()
    {
        if (_promptToPlants[GameManager.Instance.CurrentStage].plants.Count > 0)
        {
            List<PlantToWater> _currentStagePlants = _promptToPlants[GameManager.Instance.CurrentStage].plants;
            // check if all plants in photo are in bloom
            for (int i = 0; i < _currentStagePlants.Count; i++)
            {
                PlantToWater _plant = _currentStagePlants[i];
                if (_plant.isWilted)
                {
                    Debug.Log("Plant not watered");
                    return false;
                }
            }
        } 
        
        return true;
        
    }

    public override bool validatePhoto()
    {
        Vector3 attemptPos = _camera.transform.position;
        Vector3 attemptAngle = _camera.transform.eulerAngles;
        Debug.Log(string.Format("POS: {0} ANG: {1}", attemptPos, attemptAngle));

        if (base.IsAttemptAcceptable(attemptPos, attemptAngle) && _additionalValidation())
        {
            Debug.Log("REWARD");
            StartCoroutine(PhotoMatchesPrompt());
            GameManager.Instance.PromptSolved();
            return true;
        }
        else
        {
            StartCoroutine(DisplayPhoto());
            Debug.Log("NO REWARD");
            return false;
        }
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