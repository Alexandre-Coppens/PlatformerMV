using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField] private bool activated = false;
    [SerializeField] private float timeToFall = 2;
    [SerializeField] private float timeToRespawn = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            StartCoroutine(Falling());
        }
    }


    private IEnumerator Falling()
    {
        activated = true;
        Color color = GetComponent<SpriteRenderer>().color;
        while(color.a > 0)
        {
            color.a -= 1 /(timeToFall / Time.deltaTime);
            GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(timeToRespawn);
        GetComponent<Collider2D>().enabled = true;
        color.a = 1;
        GetComponent<SpriteRenderer>().color = color;
        activated = false;
    }
}