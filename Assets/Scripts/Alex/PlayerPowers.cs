using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] GameObject world;
    [SerializeField] GameObject grapple;

    [SerializeField] private float _speed;

    [SerializeField] private float grappleDist;

    private bool canChangeGravity = true;
    private bool gravityInverted = false;
    public bool newGravity = false;

    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform feet, headTop;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInputs();
        if (newGravity != gravityInverted)
        {
            _rb.velocity = new Vector2(0, -10);
            InvertGravity();
        }

        if (gravityInverted)
        {
            float horizontalMovement;

            if (Input.GetKey(KeyCode.A))
            {
                horizontalMovement = _speed * 2;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalMovement = - _speed * 2;
                _spriteRenderer.flipX = true;
            }
            else
            {
                horizontalMovement = 0;
            }

            transform.position += new Vector3(horizontalMovement * Time.deltaTime, 0, 0);
        }

        CheckGround();
    }

    private void KeyInputs()
    {
        // evil floating point bit level hacking
        if (Input.GetKeyDown(KeyCode.Tab) && canChangeGravity)
        {
            InvertGravity();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 worldPos = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mousePos = Camera.main.WorldToViewportPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Vector2 mouseDirection = (mousePos - worldPos).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDirection, grappleDist, groundLayers);
            Debug.DrawRay(transform.position, mouseDirection * grappleDist, Color.white, 10);
            if (hit)
            {
                GetComponent<DistanceJoint2D>().enabled = true;
                grapple.GetComponent<PlayerGrapple>().StartGrappling(hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && GetComponent<DistanceJoint2D>().enabled)
        {
            GetComponent<DistanceJoint2D>().enabled = false;
            grapple.GetComponent<PlayerGrapple>().KillGrappling();
        }
    }

    public void InvertGravity()
    {
        gravityInverted = !gravityInverted;
        newGravity = gravityInverted;
        world.transform.RotateAround(_rb.worldCenterOfMass + new Vector2(0, -0.1f), new Vector3(0, 0, 1), 180);
        grapple.transform.RotateAround(_rb.worldCenterOfMass + new Vector2(0, -0.1f), new Vector3(0, 0, 1), 180);
        camera.transform.rotation *= Quaternion.Euler(0, 0, 180);
        gameObject.GetComponent<SpriteRenderer>().flipX = gravityInverted;
        _rb.velocity = new Vector2(_rb.velocity.x, -_rb.velocity.y);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.transform.position, Vector2.down, 0.1f, groundLayers);
        if (hit)
        {
            canChangeGravity = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grappleDist);
    }
}
