using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CenterOfMass : MonoBehaviour
{
    public new Rigidbody rigidbody;

    public GameObject centerLocation;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = rigidbody.transform.InverseTransformPoint(centerLocation.transform.position);
    }
}