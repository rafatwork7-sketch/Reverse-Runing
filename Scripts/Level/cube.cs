using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        // Cache rigidbody reference
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GameStrings.WallJump))
        {
            rb.AddForce(-transform.forward * 1400f);
        }
    }
}
