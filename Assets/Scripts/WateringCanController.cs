using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Animator _promptAnimator;
    private IPlantToWater _selectedPlant;
    [SerializeField] private AudioSource _growSound;
    [SerializeField] private AudioSource _wateringSound;
    [SerializeField] private GameObject _heldWateringCan;
    private Animator wateringCanAnimator;
    [SerializeField] GameObject _camera;

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
        }

        Ray r = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, _wateringRange))
        {
            if (hitInfo.collider.gameObject.CompareTag("Watering Can"))
            {
                HighlightWateringCan();

                if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Xbox_X_Button"))
                {
                    Debug.Log("Watering Can");
                    _canWater = true;
                    watering_can.SetActive(false);
                }
            }

            if (hitInfo.collider.gameObject.TryGetComponent(out IPlantToWater plant) && _canWater)
            {
                _selectedPlant = plant;
                _selectedPlant.Highlight();

                if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Xbox_X_Button"))
                {
                    StartCoroutine(WaterPlant());
                    StartCoroutine(GrowPlant(plant));
                }
                
            }
        }

    }

    private IEnumerator WaterPlant()
    {
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
        Outline outline = watering_can.GetComponent<Outline>();
        if (outline != null && outline.enabled)
        {
            outline.enabled = false;
        }
    }
}
