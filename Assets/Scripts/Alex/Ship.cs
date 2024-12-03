using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private GameObject interactionKey;
    [SerializeField] private int loadLevel;

    private bool wait;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactionKey.SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            if (!inventory.IsInInventory("Key")) { return; }
            {
                if (Input.GetKey(KeyCode.E))
                {
                    inventory.RemoveItemFromInventory("Key");
                    SceneManager.LoadScene(loadLevel);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
            interactionKey.SetActive(false);
    }
}
