using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public List<Rigidbody> rigidbodies;

    void Start()
    {
        if (rigidbodies == null || rigidbodies.Count == 0)
        {
            UnityEngine.Debug.LogError("Rigidbodies list not assigned or is empty in GravityController script on " + gameObject.name);
        }
        else
        {
            foreach (var rb in rigidbodies)
            {
                if (rb == null)
                {
                    UnityEngine.Debug.LogError("One of the Rigidbody in the list is not assigned in GravityController script on " + gameObject.name);
                }
                else
                {
                    // Optional: Disable gravity at start for all objects
                    rb.useGravity = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var rb in rigidbodies)
            {
                if (rb != null)
                {
                    rb.useGravity = true;
                    UnityEngine.Debug.Log("Gravity enabled for " + rb.gameObject.name);
                }
            }
        }
    }
}
