using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private ContactFilter2D contactFilter;

    [Header("Debug")]
    [SerializeField] private bool isGoingRight = true;
    [SerializeField] private GameObject leftStop;
    [SerializeField] private GameObject rightStop;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, new Vector2(-1, 0), maxDistance);
        if (ray.Length >1)
        {
            leftStop.transform.position = ray[1].point + new Vector2(transform.localScale.x / 2, 0);
        }
        else
        {
            leftStop.transform.position = transform.position - new Vector3(maxDistance, 0);
        }

        ray = Physics2D.RaycastAll(transform.position, new Vector2(1, 0), maxDistance);
        if (ray.Length >1)
        {
            rightStop.transform.position = ray[1].point - new Vector2(transform.localScale.x / 2, 0);
            Debug.Log(ray[1].transform.name);
        }
        else
        {
            rightStop.transform.position = transform.position + new Vector3(maxDistance, 0);
        }
    }

    void Update()
    {
        spriteRenderer.flipX = !isGoingRight;

        float distLeft = Vector3.Distance(transform.position, leftStop.transform.position);
        float distRight = Vector3.Distance(transform.position, rightStop.transform.position);
        float newSpeed = Mathf.Clamp( distLeft > distRight?distRight:distLeft , speed * 0.5f, speed);
        transform.position += new Vector3(isGoingRight ? newSpeed : -newSpeed, 0) * Time.deltaTime;

        if (transform.position.x < leftStop.transform.position.x || transform.position.x > rightStop.transform.position.x)
        {
            isGoingRight = !isGoingRight;
        }
    }
}
