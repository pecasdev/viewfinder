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

    void Start()
    {
        _camera = GameObject.FindGameObjectsWithTag("1st person camera")[0];
        Debug.Log("Plant Photo");
      
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
            GameManager.Instance.PromptSolved();
            return true;
        }
        else
        {
            Debug.Log("NO REWARD");
            return false;
        }
    }

}