using UnityEngine;

namespace Weapons
{
    public class GravitySphere : MonoBehaviour
    {
        [SerializeField] float force = 10f;

        void Update()
        {
            float radius = 2f; // Set the radius of your OverlapSphere here

            // Get all colliders within a certain radius of this object
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

            // Iterate over each collider
            foreach (var collider in colliders)
            {
                // Check if the collider is tagged as "Sphere"
                if (collider.gameObject.CompareTag("Sphere"))
                {
                    // Apply a continuous force to the collider's Rigidbody
                    collider.GetComponent<Rigidbody>()
                        .AddForce((transform.position - collider.transform.position).normalized * force,
                            ForceMode.Force);
                }
            }
            Destroy(gameObject, 5f);
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collided with: " + collision.gameObject);

            if (collision.gameObject.CompareTag("Sphere"))
            {
                Destroy(collision.gameObject);
            }
        }
        
        void OnDrawGizmos()
        {
            var radius = 2f; 
            
            // Set the color of the gizmo
            Gizmos.color = Color.red;

            // Draw a wire sphere at the position of the object
            Gizmos.DrawWireSphere(transform.position, radius);
        }

    }
}