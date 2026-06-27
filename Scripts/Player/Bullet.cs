using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float bulletSpeed = 5f;

    private void OnEnable()
    {
        // Reset bullet velocity every time the object is reused
        rb.linearVelocity = transform.forward * -bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameStrings.Wall))
        {
            gameObject.SetActive(false);
        }
    }
}