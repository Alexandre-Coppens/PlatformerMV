using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField] bool gravityUP = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.GetComponent<PlayerPowers>() != null)
            {
                collision.GetComponent<PlayerPowers>().newGravity = gravityUP;
            }
        }
    }
}
