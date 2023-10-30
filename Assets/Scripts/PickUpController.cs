using System.Collections;
using System.Collections.Generic;
using UnityEngine;



interface IObjectToPickUp
{
    public void PickUp(Transform holdArea);
    public void PutDown();

    public void Move(Transform holdArea, float pickUpForce);
}

// Attach to player camera
public class PickUpController : MonoBehaviour
{
    [SerializeField] private Transform _holdArea;
    [SerializeField] private float _pickupRange = 5.0f;
    [SerializeField] private float _pickupForce = 150.0f;
    private IObjectToPickUp heldObj;
    


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            Debug.Log("E");
            if (heldObj == null)
            {
                Ray r = new Ray(transform.position, transform.forward);
                Debug.DrawRay(transform.position, transform.forward, Color.green);

                if (Physics.Raycast(r, out RaycastHit hitInfo, _pickupRange))
                {
                    Debug.Log("Raycast");
                    if (hitInfo.collider.gameObject.TryGetComponent(out IObjectToPickUp pickObj))
                    {
                        Debug.Log("hit an interactable");
                        heldObj = pickObj;
                        pickObj.PickUp(_holdArea);
                    }
                }
            } else
            {
                
                heldObj.PutDown();
                heldObj = null;

            }
        }

        if (heldObj != null)
        {
            heldObj.Move(_holdArea, _pickupForce);
        }
        
    }
}
