using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private int lives;
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;

    [SerializeField] private GameObject target;

    private float _distance;
    
    [Header("Enemy Colliders")]
    [SerializeField] private Collider2D enemyCollider;
    [SerializeField] private Collider2D weakEnemyCollider;

    [Header("Animation")] 
    [SerializeField] private AnimatorEnemyData animatorData;
    [SerializeField] private Animator animator;

    private bool _isAlive = true;
    //private bool _isHit;
    private bool _isPlayerDetected;

    private void Start()
    {
        animator.SetBool(animatorData.s_alive, _isAlive);
    }

    private void Update()
    {
        if (_isAlive && target)
        {
            _distance = Vector2.Distance(transform.position, target.transform.position);
            PlayerDetection(_distance);
        }
    }

    private void PlayerDetection(float distance)
    {
        if (_distance < detectionRange*2)
        {
            Attack();
            _isPlayerDetected = true;
        }
    }

    private void Attack()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        
        animator.SetBool(animatorData.s_playerDetected, _isPlayerDetected);
    }
    
    private void EnemyTakeDamage()
    {
        if (_isAlive)
        {
            animator.SetTrigger(animatorData.s_hit);
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
        animator.SetBool(animatorData.s_alive, false);
        
        yield return new WaitForSeconds(delay);
        
        gameObject.SetActive(false);
    }

    public void TakeDamage()
    {
        EnemyTakeDamage();
    }
}
