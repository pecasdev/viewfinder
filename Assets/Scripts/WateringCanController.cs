using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IPlantToWater
{
    public void Bloom();
}


// Attach to player camera
public class WateringCanController : MonoBehaviour
{
    [SerializeField] private float _wateringRange = 5.0f;
    private bool _canWater = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interact");
            Ray r = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, _wateringRange))
            {
                if (hitInfo.collider.gameObject.CompareTag("Watering Can"))
                {
                    Debug.Log("Watering Can");
                    _canWater = true;
                    GameObject.FindGameObjectWithTag("Watering Can").SetActive(false);
                }
                if (hitInfo.collider.gameObject.TryGetComponent(out IPlantToWater plant) && _canWater)
                {
                    plant.Bloom();
                }
            }
        }

    }
}
