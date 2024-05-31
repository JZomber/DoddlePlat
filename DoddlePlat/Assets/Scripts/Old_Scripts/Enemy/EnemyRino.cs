using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRino : MonoBehaviour
{
    public LayerMask playerLayer; // Capa del jugador
    public float detectionRange; // Rango de detección
    public GameObject raycastOrigin;

    public float speed;

    private float _currentSpeed;

    public float knockbackForce;
    
    private Animator _animator;

    private EnemyDamage _enemyDamage;
    
    private bool _playerDetected;

    private Rigidbody2D _rigidbody2D;
    
    private bool _facingRight = true;

    private int _direction = 1;

    private bool _isRunning;

    private bool _wallCollided;

    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _enemyDamage = GetComponent<EnemyDamage>();

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _currentSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyDamage.currentLives > 0)
        {
            if (PlayerDetection(detectionRange) && !_isRunning)
            {
                _animator.SetBool("playerDetected", true);
                _isRunning = true;
            }
        }
        else
        {
            OnEnemyDeath();
        }
    }

    private void FixedUpdate()
    {
        if (_isRunning)
        {
            _rigidbody2D.velocity = new Vector2(_direction * _currentSpeed, _rigidbody2D.velocity.y);
        }
    }

    private bool PlayerDetection(float range)
    {
        bool value = false;
        
        // Lanzar un raycast hacia adelante para detectar al jugador
        RaycastHit2D raycast  = Physics2D.Raycast(raycastOrigin.transform.position, transform.right, range, playerLayer);

        // Si el jugador está dentro del rango y el temporizador entre ataques ha pasado
        if (raycast.collider)
        {
            _playerDetected = raycast.collider.CompareTag("Player");
            if (_playerDetected)
            {
                value = true;
                Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * detectionRange, Color.green);
            }
            else
            {
                value = false;
            }
        }
        else
        {
            Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * detectionRange, Color.red);
        }
        
        return value;
    }

    private void WallHit()
    {
        if (_facingRight)
        {
            _rigidbody2D.velocity = new Vector2(-knockbackForce * gameObject.transform.position.x, _rigidbody2D.velocity.y);
            Flip();
            _direction = -_direction;
            detectionRange = Mathf.Abs(detectionRange);
        }
        else 
        {
            _rigidbody2D.velocity = new Vector2(knockbackForce * gameObject.transform.position.x, _rigidbody2D.velocity.y);
            Flip();
            _direction = Mathf.Abs(_direction);;
            detectionRange = -detectionRange;
        }

        if (_wallCollided)
        {
            _spriteRenderer.color = Color.red;
            _currentSpeed *= 2;
        }
        else
        {
            _spriteRenderer.color = Color.white;
            _currentSpeed = speed;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            _animator.SetTrigger("wallHit");
            _animator.SetBool("playerDetected", false);

            if (!_wallCollided)
            {
                _wallCollided = true;
            }
            else
            {
                _wallCollided = false;
            }
            
            _isRunning = false;
            WallHit();
        }
    }

    private void OnEnemyDeath()
    {
        _isRunning = false;
        _rigidbody2D.velocity = new Vector2(0, 0);
        raycastOrigin.GameObject().SetActive(false);
    }
    
    private void Flip() // Invierte el sprite según la dirección
    {
        _facingRight = !_facingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * detectionRange);
    }
}
