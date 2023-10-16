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
    [SerializeField] private bool _canWater = false;
    private GameObject watering_can;

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
            
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (hitInfo.collider.gameObject.CompareTag("Watering Can"))
                {
                    Debug.Log("Watering Can");
                    _canWater = true;
                    watering_can.SetActive(false);
                }

                if (hitInfo.collider.gameObject.TryGetComponent(out IPlantToWater plant) && _canWater)
                {
                    plant.Bloom();
                }
            }
        }

    }
}
