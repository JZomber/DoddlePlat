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
    private float _moveInput;
    [SerializeField] private int extraJumpValue;
    private int _extraJumpCount;
    
    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadious;
    [SerializeField] private LayerMask whatIsGround; 
    private bool _isGrounded;

    [Header("Taking Damage")]
    public bool canMove;
    [SerializeField] private Vector2 knockbackDir;
    [SerializeField] private int  bounceForce;
    
    
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
        if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount > 0 && !_isGrounded && canMove) // Input de salto
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
        
        if (!canMove)
        {
            _moveInput = 0;
            animator.SetFloat("Speed", _moveInput);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            _moveInput = Input.GetAxis("Horizontal");
            _rigidbody2D.velocity = new Vector2(_moveInput * speed, _rigidbody2D.velocity.y);
            
            animator.SetFloat("Speed", Mathf.Abs(_moveInput));
        }

        if (!_facingRight && _moveInput > 0)
        {
            Flip();
        }
        else if (_facingRight && _moveInput < 0)
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
                    _extraJumpCount = extraJumpValue; // Reinicia el número de saltos extras al tocar un layer que cuente como "Piso"
                }
            }
        }
    }

    public void KnockBack(GameObject gameObject) // KnockBack cuando el player recibe un ataque enemigo
    {
        if (_facingRight)
        {
            _rigidbody2D.velocity = new Vector2(-knockbackDir.x * gameObject.transform.position.x, knockbackDir.y);
        }
        else 
        {
            _rigidbody2D.velocity = new Vector2(knockbackDir.x * gameObject.transform.position.x, knockbackDir.y);
        }
    }

    public void Bounce() // Cuando el player colisiona con un "WeakPoint" enemigo
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, bounceForce);
    }

    public void OnLanding() // Cada vez que toca el suelo o una plataforma
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isGrounded", true);
    }

    private void Flip() // Invierte el sprite del player según la dirección
    {
        _facingRight = !_facingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
