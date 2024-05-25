using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveInput;
    [SerializeField] private int extraJumpValue;
    private int _extraJumpCount;
    
    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadious;
    [SerializeField] private LayerMask whatIsGround;
    private bool _isGrounded;

    private Rigidbody2D _rigidbody2D;
        
    private bool _facingRight = true;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _extraJumpCount = extraJumpValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount > 0)
        {
            _rigidbody2D.velocity = Vector2.up * jumpForce;
            _extraJumpCount--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount == 0 && _isGrounded)
        {
            _rigidbody2D.velocity = Vector2.up * jumpForce;
        }

        if (_isGrounded)
        {
            _extraJumpCount = extraJumpValue;
        }
    }

    void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);
        
        moveInput = Input.GetAxis("Horizontal");
        _rigidbody2D.velocity = new Vector2(moveInput * speed, _rigidbody2D.velocity.y);

        if (!_facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (_facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        _facingRight = !_facingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }
}
