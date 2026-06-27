using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Lifetime")]
    [SerializeField] private float destroyDelay = 0.2f;

    [Header("Physics")]
    [SerializeField] private List<Rigidbody> rigidbodies = new();
    [SerializeField] private float explosionPower = 200f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private Vector3 explosionOffset;


    private void Start()
    {
        // Play wall explosion sound
        AudioManager.PlaySound(audioSource, AudioManager.Instance.wallExplosion);

        ApplyExplosionForce();


        // Remove explosion effect after a short delay
        Destroy(gameObject, destroyDelay);
    }


    private void ApplyExplosionForce()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddExplosionForce(
                explosionPower,
                transform.position + explosionOffset,
                explosionRadius);
        }
    }
}