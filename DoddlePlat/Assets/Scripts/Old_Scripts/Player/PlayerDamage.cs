using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        _playerSpawn = gameObject.transform.position;
        
        animator.SetTrigger("isRespawn");
    }

    private void TakeDamage(Vector2 origin)
    {
        animator.SetTrigger("isHit");
        StartCoroutine(LoseControl(1f));
        playerMovement.KnockBackPoint(origin);
    }

    private IEnumerator LoseControl(float delay)
    {
        playerMovement.canMove = false;
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    private void Respawn()
    {
        gameObject.transform.position = _playerSpawn;
        animator.SetTrigger("isRespawn");
        playerMovement.canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(other.GetContact(0).normal);
        }
    }
}
