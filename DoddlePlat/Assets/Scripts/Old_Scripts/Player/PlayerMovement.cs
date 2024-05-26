using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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

    [Header("Taking Damage")]
    public bool canMove;
    [SerializeField] private Vector2 knockBackDir;
    
    
    [Header("Animation")] 
    [SerializeField] private Animator animator;
    
    private Rigidbody2D _rigidbody2D;
        
    private bool _facingRight = true;
    
    public UnityEvent onLandEvent;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _extraJumpCount = extraJumpValue;

        if (onLandEvent == null)
        {
            onLandEvent = new UnityEvent();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount > 0 && !_isGrounded && canMove)
        {
            _rigidbody2D.velocity = Vector2.up * jumpForce;
            animator.SetBool("isJumping", true);
            animator.SetBool("isGrounded", false);
            _extraJumpCount--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && canMove)
        {
            _rigidbody2D.velocity = Vector2.up * jumpForce;
            animator.SetBool("isJumping", true);
            animator.SetBool("isGrounded", false);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            moveInput = Input.GetAxis("Horizontal");
            _rigidbody2D.velocity = new Vector2(moveInput * speed, _rigidbody2D.velocity.y);
            
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        if (!_facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (_facingRight && moveInput < 0)
        {
            Flip();
        }
        
        bool wasGrounded = _isGrounded;
        _isGrounded = false;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadious, whatIsGround);
        
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isGrounded = true;
                if (!wasGrounded)
                {
                    onLandEvent.Invoke();
                    _extraJumpCount = extraJumpValue;
                }
            }
        }
    }

    public void KnockBackPoint(Vector2 origin)
    {
        _rigidbody2D.velocity = new Vector2(-knockBackDir.x * origin.x + 1, knockBackDir.y);
    }

    public void Bounce()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, knockBackDir.y);
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isGrounded", true);
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
