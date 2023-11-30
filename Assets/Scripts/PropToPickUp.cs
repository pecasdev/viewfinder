using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropToPickUp : MonoBehaviour, IObjectToPickUp
{
    private Transform _objRB;
    RigidbodyConstraints originalConstraints;
    bool originalGravity;
    
    void Start()
    {
        _objRB = GetComponent<Transform>();
        //originalConstraints = _objRB.constraints;
        //originalGravity = _objRB.useGravity;
    }

    public void PickUp(Transform holdArea)
    {
        _objRB.transform.position = holdArea.position;
        print("debug test");
    }

    public void PutDown()
    {
        _objRB.transform.parent = null;
    }

    public void Move(Transform holdArea)
    {
        _objRB.transform.position = holdArea.position;

        Vector3 holdRotation = holdArea.transform.localEulerAngles;
        holdRotation.x = _objRB.transform.localEulerAngles.x;

        _objRB.transform.localEulerAngles = holdRotation;
    }
}
