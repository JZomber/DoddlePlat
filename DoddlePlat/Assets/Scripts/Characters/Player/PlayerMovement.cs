using System;
using System.Collections;
using System.Collections.Generic;
using Command;
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
    
    private void Start()
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
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && canMove) // Input de salto
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isGrounded", false);
            
            var jumpCommand = new PhysicsJumpCommand(jumpForce, _rigidbody2D);
            EventQueue.Instance.QueueCommands(jumpCommand);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _extraJumpCount > 0 && !_isGrounded && canMove) // Input doble salto
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isGrounded", false);

            var jumpCommand = new PhysicsJumpCommand(jumpForce, _rigidbody2D);
            EventQueue.Instance.QueueCommands(jumpCommand);
            
            _extraJumpCount--;
        }
        
        if (!canMove)
        {
            _moveInput = 0;
            animator.SetFloat("Speed", _moveInput);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            _moveInput = Input.GetAxis("Horizontal");
            
            var movementCommand = new PhysicsMovementCommand(_moveInput * speed, _rigidbody2D);
            EventQueue.Instance.QueueCommands(movementCommand);
            
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
        Vector2 knockbackDirection;
        
        if (_facingRight)
        {
            knockbackDirection = new Vector2(-knockbackDir.x * gameObject.transform.position.x, knockbackDir.y);
        }
        else 
        {
            knockbackDirection = new Vector2(knockbackDir.x * gameObject.transform.position.x, knockbackDir.y);
        }

        var knockBackCommand = new PhysicsKnockBackCommand(knockbackDirection, _rigidbody2D);
        EventQueue.Instance.QueueCommands(knockBackCommand);
    }

    public void Bounce() // Cuando el player colisiona con un "WeakPoint" enemigo
    {
        var bounceCommand = new PhysicsJumpCommand(bounceForce, _rigidbody2D);
        EventQueue.Instance.QueueCommands(bounceCommand);
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
