using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class Axe : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Transform leftHand;
    public GameObject centerLocation;
    public Quaternion initialAxeRotation;

    // Start is called before the first frame update
    void Start()
    {
        // Initializes the rigidbody
        _rigidbody = GetComponent<Rigidbody>();
        
        // Sets rigidBody center of mass based on the position of the centerLocation game object
        _rigidbody.centerOfMass = _rigidbody.transform.InverseTransformPoint(centerLocation.transform.position);
    }

    void Update()
    {
        // List to store input devices
        List<InputDevice> devices = new List<InputDevice>();
        
        // Get input devices with left controller characteristics
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
        
        // Check if any devices are found
        if (devices.Count > 0)
        {
            InputDevice device = devices[0];
            
            // Check if the X button is pressed
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) &&
                primaryButtonValue)
            {
                // The X button was pressed, retrieve the axe to left hand
                RetrieveAxe();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // If the axe collided with the target, make axe kinematic
        if (collision.gameObject.CompareTag("Target"))
        {
            _rigidbody.isKinematic = true;
        }
    }
    
    // Retrieves the axe by setting the rigidbody to non-kinematic and lerping it to the hand position.
    // ReSharper disable Unity.PerformanceAnalysis  
    private void RetrieveAxe()
    {
        _rigidbody.isKinematic = false;
        initialAxeRotation = transform.rotation;
        StartCoroutine(LerpToHand(0.5f));
    }
    
    // Lerps the object to the left hand position over a specified time (0.5 seconds).
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator LerpToHand(float time)
    {
        // Get the initial position of the object
        Vector3 start = transform.position;
           
        // Get the target position (left hand position)
        Vector3 end = leftHand.position;
             
        // Initialize the elapsed time
        float elapsedTime = 0;
            
        // Loop until the elapsed time reaches the specified time
        while (elapsedTime < time)
        { 
            // Lerp the object's position based on the elapsed time
            transform.position = Vector3.Lerp(start, end, (elapsedTime / time));
            
            // Create a new rotation with a 90 degree offset on the Z axisrm.rotation
            Quaternion targetRotation = leftHand.rotation * Quaternion.Euler(15, 0, -90);
            
            // Lerp the rotation
            transform.rotation = Quaternion.Lerp(initialAxeRotation, targetRotation, (elapsedTime / time));
            
            // Update the elapsed time
            elapsedTime += Time.deltaTime;                                   
               
            // Wait for the next frame
            yield return null;
        }
            
        // Ensure the final position is the leftHand position
        transform.position = end;
    }
}