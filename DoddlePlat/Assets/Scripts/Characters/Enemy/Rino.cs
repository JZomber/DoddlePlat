using System;
using System.Collections;
using UnityEngine;

public class Rino : MoveAbleEnemy, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float knockbackForce;
    private float _currentSpeed;

    [HideInInspector] public int Lives => lives;
    
    [Header("Detection & Attack")]
    [SerializeField] private LayerMask playerLayer; // Capa del jugador
    [SerializeField] private float detectionRange;
    [SerializeField] private GameObject raycastOrigin;
    
    private const string s_WallHit = "wallHit";
    private bool _isHit;
    private bool _isRunning;
    private bool _wallCollided;
    
    [Header("Others")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool _facingRight = true;
    private int _direction = 1;

    public event Action OnTakeDamage;
    public event Action OnBossKilled;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        _currentSpeed = speed;
        
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            if (PlayerDetection(detectionRange))
            {
                animator.SetBool(animatorData.s_playerDetected, _isPlayerDetected);
                _isRunning = true;
            }
            else
            {
                animator.SetBool(animatorData.s_playerDetected, false);
            }
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
            _isPlayerDetected = raycast.collider.CompareTag("Player");
            if (_isPlayerDetected)
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
            animator.SetTrigger(s_WallHit);
            animator.SetBool(animatorData.s_playerDetected, false);

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
    
    private void EnemyTakeDamage()
    {
        if (_isAlive)
        {
            animator.SetTrigger(animatorData.s_hit);
            lives--;
            
            OnTakeDamage?.Invoke();
            
            if (lives <= 0)
            {
                _isAlive = false;
                StartCoroutine(EnemyDeath(2f));
            } 
        }
    }
    
    private IEnumerator EnemyDeath(float delay)
    {
        _isRunning = false;
        enemyCollider.enabled = false;
        weakEnemyCollider.enabled = false;
        animator.SetBool(animatorData.s_alive, false);
        _rigidbody2D.velocity = new Vector2(0, 0);
        _rigidbody2D.isKinematic = true;
        
        OnBossKilled?.Invoke();
        
        yield return new WaitForSeconds(delay);
        
        gameObject.SetActive(false);
    }
    
    public void TakeDamage()
    {
        EnemyTakeDamage();
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
