using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyTurtle : MonoBehaviour
{
    [FormerlySerializedAs("_DamageCollider")]
    [Header("Enemy Colliders")]
    [SerializeField] private GameObject _damageCollider;
    [SerializeField] private Collider2D _DetectionCollider;
    
    [Header("Animation")]
    [SerializeField] public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _damageCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("playerDetected", true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("playerDetected", false);
        }
    }
}
