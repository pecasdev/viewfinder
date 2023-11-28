using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


interface IPlantToWater
{
    public void Bloom();
    public void Highlight();

    public void RemoveHighlight();
}


// Attach to player camera
public class WateringCanController : MonoBehaviour
{
    [SerializeField] private float _wateringRange = 5.0f;
    [SerializeField] private bool _canWater = false;
    private GameObject watering_can;
    private IPlantToWater _selectedPlant;
    [SerializeField] private AudioSource _growSound;
    [SerializeField] private AudioSource _wateringSound;
    [SerializeField] private GameObject _heldWateringCan;
    private Animator wateringCanAnimator;
    [SerializeField] GameObject _camera;
    [SerializeField] Image wateringStatusUI;
    [SerializeField] TextMeshProUGUI mechanicInfoText;
    [SerializeField] Sprite checkmarkSprite;
    [SerializeField] private TextMeshProUGUI hoverText;
    [SerializeField] private GameObject hoverCanvas;

    private void Start()
    {
        watering_can = GameObject.FindGameObjectWithTag("Watering Can");
        wateringCanAnimator = _heldWateringCan.GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!_canWater)
        {
            RemoveWateringCanHighlight();
        }
        if (_selectedPlant != null)
        {
            _selectedPlant.RemoveHighlight();
            _selectedPlant = null;
            hoverCanvas.SetActive(false);
        }

        Ray r = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, _wateringRange))
        {
            if (hitInfo.collider.gameObject.CompareTag("Watering Can"))
            {
                HighlightWateringCan();

                if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Xbox_X_Button"))
                {
                    RemoveWateringCanHighlight();
                    _canWater = true;
                    wateringStatusUI.sprite = checkmarkSprite;
                    mechanicInfoText.text = "Water Plant (highlighted)";
                    watering_can.SetActive(false);
                }
            }

            if (hitInfo.collider.gameObject.TryGetComponent(out PlantToWater plant) && _canWater)
            {
                _selectedPlant = plant;
                if (plant.isWilted)
                {
                    _selectedPlant.Highlight();
                    hoverCanvas.SetActive(true);
                    hoverText.text = "Water";


                    if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Xbox_X_Button"))
                    {
                        StartCoroutine(WaterPlant());
                        StartCoroutine(GrowPlant(plant));
                        
                    }
                }
                
            }
        }

    }

    private IEnumerator WaterPlant()
    {
        hoverCanvas.SetActive(false);
        _camera.SetActive(false);
        _heldWateringCan.SetActive(true);
        wateringCanAnimator.Play("WaterPlant");
        yield return new WaitForSeconds(1f);
        _wateringSound.Play();
        yield return new WaitForSeconds(1f);
        _camera.SetActive(true);
        _heldWateringCan.SetActive(false);

    }
    private IEnumerator GrowPlant(IPlantToWater plant)
    {
        plant.RemoveHighlight();
        yield return new WaitForSeconds(1.5f);
        _growSound.Play();
        plant.Bloom();

    }

    private void HighlightWateringCan()
    {
        hoverCanvas.SetActive(true);
        hoverText.text = "Pick up";
        Outline outline = watering_can.GetComponent<Outline>();
        if (outline != null)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;
            outline.enabled = true;
        } else
        {
            Debug.Log("creating outline");
            outline = watering_can.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5f;

        }
    }

    private void RemoveWateringCanHighlight()
    {
        hoverCanvas.SetActive(false);
        Outline outline = watering_can.GetComponent<Outline>();
        if (outline != null && outline.enabled)
        {
            outline.enabled = false;
        }
    }
}
