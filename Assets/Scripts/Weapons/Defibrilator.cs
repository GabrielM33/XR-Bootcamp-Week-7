using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace Weapons
{
    public class Defibrilator : Gun
    {
        private const float ForceMagnitude = 10f;
        
        [SerializeField] private Rigidbody[] spheres;
        [SerializeField] private float force = ForceMagnitude;
        [SerializeField] private DefibrilatorMode mode = DefibrilatorMode.Push;
        
        private bool _isTriggerHeld;
        
        private enum DefibrilatorMode { Push, Pull }
    
        protected override void Start()
        {
            var activeAmmoSocket = GetComponentInChildren<XRTagLimitedSocketInteractor>();
            _ammoSocket = activeAmmoSocket;
            
            base.Start();
            Assert.IsNotNull(spheres, "You have not assigned spheres to defibrilator" + name);
        }
        
         protected override void Fire(ActivateEventArgs arg0)
    {
        Debug.Log("Fire method called");
        
        if (!CanFire()) return;

        base.Fire(arg0);

        // Set isTriggerHeld to true when the trigger is pressed
        _isTriggerHeld = true;
    }
         
    void Update()
    {
        // Apply forces if the trigger is being held down
        if (_isTriggerHeld)
        {
            ApplyGravityForces();
        }
    }
        
        private void ApplyGravityForces()
        {
            Debug.Log("Applying Gravity");
            
            float radius = 1f; // Set the radius of your SphereCast here

            // Cast a sphere from the transform's position in the direction of its forward vector
            if (Physics.SphereCast(transform.position + 1f * transform.forward, radius, transform.forward, out RaycastHit hit) && hit.rigidbody != null)
            {
                ApplyForceToSpheres();
            } 
        }
        
        private void ApplyForceToSpheres()
        {
            foreach (var sphere in spheres)
            {
                if (mode == DefibrilatorMode.Push)
                {
                    ApplyPushForce();
                }
                else if (mode == DefibrilatorMode.Pull)
                {
                    ApplyPullForce();
                }
            }
        }
        
        private void ApplyPushForce()
        {
            float radius = 2f;

            Collider[] colliders = Physics.OverlapSphere(_gunBarrel.position + 4f * _gunBarrel.forward, radius);

            // Iterate over each collider
            foreach (var coll in colliders)
            {
                if (coll.gameObject.CompareTag("Sphere"))
                {
                    Rigidbody rb = coll.GetComponent<Rigidbody>();
                    rb.AddForce((_gunBarrel.position + 4f * _gunBarrel.forward - coll.transform.position).normalized * force, ForceMode.Force);
                }
            }
        }

        private void ApplyPullForce()
        {
            float radius = 4f; 
            
            Collider[] colliders = Physics.OverlapSphere(_gunBarrel.position, radius);

            // Iterate over each collider
            foreach (var coll in colliders)
            {
                if (coll.gameObject.CompareTag("Sphere"))
                {
                    Rigidbody rb = coll.GetComponent<Rigidbody>();
                    rb.AddForce((coll.transform.position - _gunBarrel.position + 4f * _gunBarrel.forward).normalized * force, ForceMode.Force);
                }
            }
        }
    
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            float radius = 4f; // Set the radius of your SphereCast here

            // Draw the sphere at the start of the cast
            var transform1 = transform;
            Gizmos.DrawWireSphere(transform1.position + 4f * transform1.forward, radius);
        }
    }
}
