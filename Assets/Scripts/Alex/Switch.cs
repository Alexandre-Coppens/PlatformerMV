using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Animator bridgeAnimator;
    [SerializeField] private GameObject interactionKey;
    [SerializeField] private Sprite repairedTexture;
    [SerializeField] private Sprite usedTexture;

    public bool isBroken = true;
    public bool isUsed = false;

    private bool wait;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isUsed)
        {
            interactionKey.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.W) && !isUsed && !wait) 
            {
                PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
                if (inventory == null) { return; }
                if (!isBroken)
                {
                    isUsed = true;
                    gameObject.GetComponent<SpriteRenderer>().sprite = usedTexture;
                    bridgeAnimator.SetTrigger("Down");
                }
                if (inventory.IsInInventory("LEVERHANDLE") && isBroken)
                {
                    inventory.RemoveItemFromInventory("LEVERHANDLE");
                    isBroken = false;
                    gameObject.GetComponent<SpriteRenderer>().sprite = repairedTexture;
                    wait = true;
                }
            }
            if (!Input.GetKey(KeyCode.W))
            {
                wait = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isUsed)
        {
            interactionKey.SetActive(false);
        }
    }
}
