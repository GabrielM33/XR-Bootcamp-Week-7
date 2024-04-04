using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Weapons
{
    public class GrenadeLauncher : Gun
    {
        [SerializeField] private GameObject gravitySphere;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private LineRenderer lineRenderer;
        protected AmmoClip _ammoClip;
        protected override void Start()
        {
            var activeAmmoSocket = GetComponentInChildren<XRTagLimitedSocketInteractor>();
            _ammoSocket = activeAmmoSocket;
            
            base.Start();

            // Initialize the LineRenderer
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
        }
        
        void Update()
        {
            // Set the start position of the line
            lineRenderer.SetPosition(0, _gunBarrel.transform.position);

            // Calculate the end position of the line
            Vector3 endPosition = _gunBarrel.transform.position + _gunBarrel.transform.forward * 10f;
            

            // Set the end position of the line
            lineRenderer.SetPosition(1, endPosition);
    
            PredictTrajectory();
        }


        protected override void Fire(ActivateEventArgs arg0)
        {
            Debug.Log("Fire method called");
            
            if (!CanFire()) return;

            base.Fire(arg0);
            
            // Instantiate the gravitySphere at the gun barrel's position
            var bullet = Instantiate(gravitySphere, _gunBarrel.position, Quaternion.identity)
                .GetComponent<Rigidbody>();
            
            // Apply force to the gravitySphere Instance
            bullet.AddForce( _gunBarrel.forward * bulletSpeed, ForceMode.Impulse);
            
            // Destroy the gravitySphereInstance after 5 seconds
            Destroy(gravitySphere.gameObject, 5f);
        }
        
        /// Predicts the trajectory of a projectile using a line renderer.
        private void PredictTrajectory()
        {
            // Number of points for the line renderer
            int numPoints = 100;
            // Time between each point
            float timeDelta = 0.01f;

            // Set the position count of the line renderer
            lineRenderer.positionCount = numPoints;

            // Calculate the velocity of the projectile
            Vector3 velocity =  _gunBarrel.transform.forward * bulletSpeed / gravitySphere.GetComponent<Rigidbody>().mass;
            // Get the initial position of the projectile
            Vector3 position = _gunBarrel.position;

            // Iterate over each point in the trajectory
            for (int i = 0; i < numPoints; i++)
            {
                // Calculate the time for the current point
                float t = timeDelta * i;

                // Check for collision with objects in the scene
                if (Physics.SphereCast(position, 0.1f, velocity, out RaycastHit hit, velocity.magnitude * t))
                {
                    // Reflect the velocity on the normal of the hit surface
                    velocity = Vector3.Reflect(velocity, hit.normal);
                    // Update the position to the point of collision
                    position = hit.point;
                }
                else
                {
                    // Calculate the new position of the projectile
                    position += velocity * t + Physics.gravity * (0.5f * t * t);
                    // Update the velocity based on gravity
                    velocity += Physics.gravity * t;
                }

                // Set the position of the line renderer for the current point
                lineRenderer.SetPosition(i, position);
            }
        }
    }
}