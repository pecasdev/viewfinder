using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPositionIfOOB : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 originalPosition;
    public Bounds bounds;

    void Start()
    {
        originalPosition = transform.position;    
    }

    // Update is called once per frame
    void Update()
    {
        if (!bounds.Contains(transform.position))
        {
            transform.position = originalPosition;
        }
    }
}
