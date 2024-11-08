using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] GameObject world;

    [SerializeField] private float _speed;
    
    public bool gravityInverted = false;

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
        // evil floating point bit level hacking
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            gravityInverted = !gravityInverted;
            world.transform.RotateAround(_rb.worldCenterOfMass, new Vector3(0, 0, 1), 180);
            camera.transform.rotation *= Quaternion.Euler(0, 0, 180);
            gameObject.GetComponent<SpriteRenderer>().flipX = gravityInverted;
            _rb.velocity = new Vector2(_rb.velocity.x, -_rb.velocity.y);
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
    }
}
