using UnityEngine;

public class Ennemy_Fly : MonoBehaviour
{

    [SerializeField] private float speed = 3;
    [SerializeField] private bool isGoingRight = false;
    [SerializeField] private int groundLayer;

    [SerializeField] private float oscilationMultiplication = 1f;

    private Vector3 startPos;
    private Rigidbody2D rb;
    private SpriteRenderer renderer;

    private void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        renderer.flipX = isGoingRight;
        rb.velocity = new Vector3(isGoingRight ? speed : -speed, Mathf.Sin(Time.time)*oscilationMultiplication + 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == groundLayer)
        {
            isGoingRight = !isGoingRight;
        }
    }
}
