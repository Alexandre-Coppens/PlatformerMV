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

    [Header("DEBUG")]
    [SerializeField] private Vector2 worldPos;
    [SerializeField] private Vector2 mousePos;
    [SerializeField] private bool hasGravity;
    [SerializeField] private bool hasGrapple;

    [Header("Classes")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private DistanceJoint2D _distanceJoint;
    [SerializeField] private PlayerGrapple _grappleScript;

    void Start()
    {
        worldPos = Vector2.zero;
        mousePos = Vector2.zero;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _distanceJoint = GetComponent<DistanceJoint2D>();
        _grappleScript = grapple.GetComponent<PlayerGrapple>();
        _playerInventory = GetComponent<PlayerInventory>();
    }

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

        if (!hasGravity)
        {
            if (_playerInventory.IsInInventory("Gravity"))
            {
                hasGravity = true;
            }
        }
        if (!hasGrapple)
        {
            if (_playerInventory.IsInInventory("Grapple"))
            {
                hasGrapple = true;
            }
        }
    }

    private void KeyInputs()
    {
        // evil floating point bit level hacking
        if (Input.GetKeyDown(KeyCode.Tab) && canChangeGravity && hasGravity)
        {
            InvertGravity();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_distanceJoint.enabled && hasGrapple)
        {
            worldPos = new Vector2(transform.position.x, transform.position.y);
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDirection = (mousePos - worldPos).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseDirection, grappleDist, groundLayers);
            Debug.DrawRay(transform.position, mouseDirection * grappleDist, Color.white, 10);
            if (hit)
            {
                _distanceJoint.enabled = true;
                _grappleScript.StartGrappling(hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && _distanceJoint.enabled)
        {
            _distanceJoint.enabled = false;
            _grappleScript.KillGrappling();
            _rb.velocity = new Vector2(Input.GetKey(KeyCode.A)?-10:(Input.GetKeyDown(KeyCode.D)?10:0), 15);
        }
    }

    public void InvertGravity()
    {
        gravityInverted = !gravityInverted;
        newGravity = gravityInverted;
        canChangeGravity = false;
        world.transform.RotateAround(_rb.worldCenterOfMass + new Vector2(0, -0.1f), new Vector3(0, 0, 1), 180);
        grapple.transform.RotateAround(_rb.worldCenterOfMass + new Vector2(0, -0.1f), new Vector3(0, 0, 1), 180);
        camera.transform.rotation *= Quaternion.Euler(0, 0, 180);
        _spriteRenderer.flipX = gravityInverted;
        _rb.velocity = new Vector2(_rb.velocity.x, -_rb.velocity.y);
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.transform.position, Vector2.down, 0.1f, groundLayers);
        if (hit && !canChangeGravity)
        {
            canChangeGravity = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grappleDist);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mousePos, 0.1f);
        Gizmos.DrawWireSphere(worldPos, 0.1f);
    }
}
