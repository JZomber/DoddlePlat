using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private int lives;
    
    [Header("Detección & Ataque")]
    [SerializeField] private LayerMask playerLayer; // Capa del jugador
    [SerializeField] private float detectionRange;
    [SerializeField] private GameObject raycastOrigin;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private GameObject bullet;
    
    [Header("Enemy Colliders")]
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Collider2D weakEnemyCollider;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private const string s_IsAlive = "isAlive";
    private const string s_IsHIt = "isHit";
    private const string s_PlayerDetected = "playerDetected";

    private bool _isAlive = true;
    private bool _isHit;
    private bool _playerDetected;
    
    private void Start()
    {
        animator.SetBool(s_IsAlive, _isAlive);
    }
    
    void Update()
    {
        if (_isAlive)
        {
            if (PlayerDetection(detectionRange))
            {
                animator.SetBool(s_PlayerDetected, _playerDetected);
            }
            else
            {
                animator.SetBool(s_PlayerDetected, false);
            }
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
    
    private void ShootBullet()
    {
        var rotation = shootOrigin.rotation;
        rotation *= Quaternion.Euler(0, 0, -90);
        Instantiate(bullet, shootOrigin.position, rotation);
    }
    
    private void EnemyTakeDamage()
    {
        if (_isAlive)
        {
            animator.SetTrigger(s_IsHIt);
            lives--;

            if (lives <= 0)
            {
                _isAlive = false;
                StartCoroutine(EnemyDeath(2f));
            } 
        }
    }
    
    private IEnumerator EnemyDeath(float delay)
    {
        enemyCollider.enabled = false;
        weakEnemyCollider.enabled = false;
        animator.SetBool(s_IsAlive, false);
        
        yield return new WaitForSeconds(delay);
        
        gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        EnemyTakeDamage();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * detectionRange);
    }
}
