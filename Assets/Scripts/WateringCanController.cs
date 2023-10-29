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
    [SerializeField] private Animator _promptAnimator;

    private void Start()
    {
        watering_can = GameObject.FindGameObjectWithTag("Watering Can");
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            PhotoAlbumManager.Instance.OpenPhotoAlbum();
            _promptAnimator.Play("PhotoMatchesPrompt");

        }

        Ray r = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, _wateringRange))
        {
            
            if (Input.GetKeyDown(KeyCode.E) || Input.GetAxis("Xbox_Y_Button") == 1)
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
