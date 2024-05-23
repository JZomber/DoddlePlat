using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed; // Velocidad de movimiento lateral
    [SerializeField] private float jumpForce; // Fuerza de salto
    [SerializeField] private bool canDoubleJump = true;
    
    private bool _isGrounded;

    [SerializeField] private float raycastLenght;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    { // Movimiento lateral
        float moveInput = Input.GetAxis("Horizontal");
        _rigidbody2D.velocity = new Vector2(moveInput * moveSpeed, _rigidbody2D.velocity.y);

        // Salto
        if (Input.GetButtonDown("Jump") && _isGrounded)
        { 
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }
        else if (Input.GetButtonDown("Jump") && !_isGrounded && canDoubleJump)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
            canDoubleJump = false;
        }
        
        //GroundRaycast();
    }

    void GroundRaycast()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down);
        Debug.DrawRay(transform.position, Vector2.down * raycastLenght, Color.red);

        if (hitGround.collider.GameObject().CompareTag("Ground"))
        {
            _isGrounded = true;
            
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }
        }
        else
        {
            _isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
