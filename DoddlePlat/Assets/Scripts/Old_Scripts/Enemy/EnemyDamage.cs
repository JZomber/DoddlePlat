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
    [SerializeField] public Animator animator;
    
    [Header("Enemy Stats")]
    [SerializeField] private int lives;
    [HideInInspector] public int currentLives;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isAlive", true);

        currentLives = lives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage() // Recibir daño por player
    {
        animator.SetTrigger("isHit");
        currentLives--;

        if (currentLives <= 0)
        {
            StartCoroutine(EnemyDeath(2f));
        }
    }

    private IEnumerator EnemyDeath(float delay) // Ejecuta animación de muerte, desactiva colisiones y GameObject
    {
        _EnemyCollider.enabled = false;
        _weakPointCollider.enabled = false;
        animator.SetBool("isAlive", false);

        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
