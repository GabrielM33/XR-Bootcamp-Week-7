using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField] private float _soundRadius = 5f;
    [SerializeField] private float _impulseThreshold = 2f;

    public UnityEvent onEmitSound;

    private float _collsionTimer = 0f;
    
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_collsionTimer < 2f)
        {
            _collsionTimer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_collsionTimer < 2f) return;
        
        if (other.impulse.magnitude > _impulseThreshold || other.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();

            onEmitSound.Invoke();
            
            Debug.Log("Sound Emitter Collided with "+other.gameObject.name);
            Collider[] _colliders = Physics.OverlapSphere(transform.position, _soundRadius);
            foreach (var col in _colliders)
            {
                if (col.TryGetComponent(out EnemyController enemyController))
                {
                    enemyController.InvestigatePoint(transform.position);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _soundRadius);
    }
}
