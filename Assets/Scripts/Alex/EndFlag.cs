using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndFlag : MonoBehaviour
{
    [SerializeField] private GameObject interdictionSprite;
    [SerializeField] private string nextLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            if (inventory == null) { return; }
            if (inventory.IsInInventory("REDGEM"))
            {
                SceneManager.LoadScene(nextLevel);
            }
            else
            {
                interdictionSprite.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interdictionSprite.SetActive(false);
    }
}
