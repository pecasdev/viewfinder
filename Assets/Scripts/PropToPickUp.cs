using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropToPickUp : MonoBehaviour, IObjectToPickUp
{
    private Rigidbody _objRB;
    RigidbodyConstraints originalConstraints;
    
    void Start()
    {
        _objRB = GetComponent<Rigidbody>();
        originalConstraints = _objRB.constraints;
    }

    public void PickUp(Transform holdArea)
    {
        _objRB.useGravity = false;
        _objRB.drag = 10;
        _objRB.constraints = RigidbodyConstraints.FreezeRotation;
        _objRB.transform.parent = holdArea;
    }

    public void PutDown()
    {
        _objRB.useGravity = true;
        _objRB.drag = 1;
        _objRB.constraints = originalConstraints;
        _objRB.transform.parent = null;
    }

    public void Move(Transform holdArea, float pickUpForce)
    {
        if (Vector3.Distance(transform.position, holdArea.position) > 1.0f)
        {
            Vector3 moveDirection = (holdArea.position - transform.position);
            _objRB.AddForce(moveDirection * pickUpForce);
        }
    }
}
