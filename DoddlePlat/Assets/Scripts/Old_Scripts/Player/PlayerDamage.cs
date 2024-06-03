using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDamage : MonoBehaviour
{
    [Header("Player Movement")] 
    [SerializeField] private PlayerMovement playerMovement;
    
    [Header("Taking Damage")] 
    [SerializeField] private float damageCD;
    
    [Header("Animation")] 
    [SerializeField] private Animator animator;

    private Vector2 _playerSpawn;

    private bool _weakPointHit;

    private bool _isHit;
    
    private BoxCollider2D _boxCollider2D;

    private Rigidbody2D _rigidbody2D;

    private LevelManager _levelManager;

    private int _playerLives;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        _playerSpawn = gameObject.transform.position;
        
        animator.SetBool("isAlive", true);
        
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _levelManager = FindObjectOfType<LevelManager>();

        _playerLives = _levelManager.playerLives;
    }

    public void PlayerSpawnRelocate()
    {
        _playerSpawn = gameObject.transform.position;
    }

    private void TakeDamage(GameObject gameObject) // Recibir daño al colisionar con enemigos
    {
        animator.SetTrigger("isHit");
        LoseControl();
        playerMovement.KnockBack(gameObject);
        _playerLives--;
    }

    private void LoseControl() // El player no puede moverse
    {
        playerMovement.canMove = false;
        animator.SetBool("isAlive", false);
        _boxCollider2D.enabled = false;
        
        StartCoroutine(Respawn(2f));
    }

    private IEnumerator Respawn(float delay) // Re-posición del player + animación
    {
        yield return new WaitForSeconds(delay);

        if (_playerLives > 0)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);
            gameObject.transform.position = _playerSpawn;
            _boxCollider2D.enabled = true;
        
            animator.SetTrigger("isRespawn");
            animator.SetBool("isAlive", true);

            StartCoroutine(PlayerCanMove(1f));
        }
    }
    
    private IEnumerator CoolDownHit(float delay) // CoolDown antes de que se pueda colisionar nuevamente
    {
        yield return new WaitForSeconds(delay);
        _weakPointHit = false;
        _isHit = false;
    }

    private IEnumerator PlayerCanMove(float delay) //Llamado en el spawn del player
    {
        yield return new WaitForSeconds(delay);
        playerMovement.canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other) // Colisión Player > Enemigos
    {
        if (other.gameObject.CompareTag("Enemy") && !_weakPointHit && !_isHit)
        {
            _isHit = true;
            TakeDamage(other.gameObject);
            StartCoroutine(CoolDownHit(1f));
            _levelManager.PlayerTakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // Colisión Player > Punto débil (Enemigo)
    {
        if (other.gameObject.CompareTag("WeakPoint"))
        {
            _weakPointHit = true;
            playerMovement.Bounce();
            other.GameObject().GetComponentInParent<EnemyDamage>().TakeDamage();
            StartCoroutine(CoolDownHit(1f));
        }

        if (other.gameObject.CompareTag("EnemyAttack") && !_isHit)
        {
            _isHit = true;
            TakeDamage(other.gameObject);
            StartCoroutine(CoolDownHit(1f));
            _levelManager.PlayerTakeDamage();
        }
    }
}
