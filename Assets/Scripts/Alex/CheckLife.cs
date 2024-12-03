using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckLife : MonoBehaviour
{

    [SerializeField] private Image life;
    private bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isAlive = life.fillAmount == 0 ? false : true;
        if (!isAlive)
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach(Collider2D collider2D in colliders)
            {
                collider2D.enabled = false;
            }
            StartCoroutine("WaitForRespawn");
        }
    }

    private IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
