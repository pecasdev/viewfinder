using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantToWater : MonoBehaviour, IPlantToWater
{
    private bool _isWilted = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bloom()
    {
        // play bloom animation
        if (_isWilted)
        {
            // play bloom animation
            _isWilted = false;
            if (transform.GetComponent<Animator>())
            {
                transform.GetComponent<Animator>().SetTrigger("Bloom");
            }
        }
    }
}
