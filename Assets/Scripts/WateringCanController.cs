using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private void Start()
    {
        watering_can = GameObject.FindGameObjectWithTag("Watering Can");
    }


    // Update is called once per frame
    void Update()
    {

        Ray r = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, _wateringRange))
        {
            RemoveWateringCanHighlight();
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
                    plant.RemoveHighlight();
                    plant.Bloom();
                }
                
            } else
            {
                if (_selectedPlant != null)
                {
                    _selectedPlant.RemoveHighlight();
                    _selectedPlant = null;
                }
                
            }

            
        }

    }


    private void HighlightWateringCan()
    {
        if (watering_can.GetComponent<Outline>() != null)
        {
            watering_can.GetComponent<Outline>().enabled = true;
        } else
        {
            Outline outline = watering_can.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5.0f;
            outline.enabled = true;
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
