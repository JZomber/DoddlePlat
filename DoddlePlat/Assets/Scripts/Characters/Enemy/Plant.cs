using System.Collections;
using UnityEngine;

public class Plant : RangedEnemy, IDamageable
{
    protected override void Start()
    {
        base.Start();
    }
    
    void Update()
    {
        if (_isAlive)
        {
            if (PlayerDetection(rangedData.detectionRange))
            {
                animator.SetBool(animatorData.s_playerDetected, _isPlayerDetected);
            }
            else
            {
                animator.SetBool(animatorData.s_playerDetected, false);
            }
        }
    }
    
    private bool PlayerDetection(float range)
    {
        bool value = false;
        
        // Lanzar un raycast hacia adelante para detectar al jugador
        RaycastHit2D raycast  = Physics2D.Raycast(raycastOrigin.transform.position, transform.right, range, rangedData.playerLayer);

        // Si el jugador está dentro del rango y el temporizador entre ataques ha pasado
        if (raycast.collider)
        {
            _isPlayerDetected = raycast.collider.CompareTag("Player");
            if (_isPlayerDetected)
            {
                value = true;
                Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * rangedData.detectionRange, Color.green);
            }
            else
            {
                value = false;
            }
        }
        else
        {
            Debug.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * rangedData.detectionRange, Color.red);
        }
        
        return value;
    }

    protected override void Shoot()
    {
        var rotation = shootOrigin.rotation;
        rotation *= Quaternion.Euler(0, 0, -90);
        Instantiate(rangedData.bulletPrefab, shootOrigin.position, rotation);
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
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * rangedData.detectionRange);
    }
}
