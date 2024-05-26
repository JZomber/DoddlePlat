using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyDamage : MonoBehaviour
{
    [Header("Enemy Colliders")]
    [SerializeField] private Collider2D _EnemyCollider;
    [SerializeField] private Collider2D _weakPointCollider;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    
    [Header("Enemy Stats")]
    [SerializeField] private int lives;

    // Start is called before the first frame update
    void Start()
    {
        _animator.SetBool("isAlive", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage() // Recibir daño por player
    {
        _animator.SetTrigger("isHit");
        lives--;

        if (lives <= 0)
        {
            StartCoroutine(EnemyDeath(2f));
        }
    }

    private IEnumerator EnemyDeath(float delay) // Ejecuta animación de muerte, desactiva colisiones y GameObject
    {
        _EnemyCollider.enabled = false;
        _weakPointCollider.enabled = false;
        _animator.SetBool("isAlive", false);

        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
