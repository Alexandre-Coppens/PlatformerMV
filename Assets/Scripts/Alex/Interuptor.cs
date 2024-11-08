using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interuptor : MonoBehaviour
{
    [SerializeField] private float activationVelocity = -5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 velocity = collision.GetComponent<Rigidbody2D>().velocity;
            Debug.Log(velocity.y);
            if (velocity.y <= activationVelocity)
            {
                Destroy(gameObject);
            }
        }
    }
}
