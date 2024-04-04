using UnityEngine;

namespace XR
{
    public class MinimapRotator : MonoBehaviour
    {
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private Transform _rightHandTransform;
        [SerializeField] private Transform _rotationReference;

        private Vector3 _initialRotation;
        private Handedness _handedness;
        
        // Start is called before the first frame update
        private void Start()
        {
            _initialRotation = transform.eulerAngles;
            _handedness = GetComponent<Handedness>();
            if (_handedness.handed == Handed.Left)
            {
                _rotationReference = _rightHandTransform;
            }
            else
            {
                _rotationReference = _leftHandTransform;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            Vector3 newRot = new Vector3(0, 0, -_rotationReference.eulerAngles.y) + _initialRotation;
            transform.rotation = Quaternion.Euler(newRot);
        }
    }
}
