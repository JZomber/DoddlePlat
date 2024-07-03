using System.Collections;
using UnityEngine;

public class Bat : MoveAbleEnemy, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float detectionRange;

    private GameObject target;

    private float _distance;

    protected override void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
        base.Start();
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
