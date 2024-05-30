using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlant : MonoBehaviour
{
    public LayerMask playerLayer; // Capa del jugador
    public float detectionRange; // Rango de detección
    public GameObject raycastOrigin;

    public Transform shootOrigin;
    public GameObject bullet;

    private Animator _animator;

    private EnemyDamage _enemyDamage;

    private bool _playerDetected;

    void Start()
    {
        _animator = GetComponent<Animator>();

        _enemyDamage = GetComponent<EnemyDamage>();
    }

    void Update()
    {
        if (_enemyDamage.currentLives > 0)
        {
            if (PlayerDetection(detectionRange))
            {
                _animator.SetBool("playerDetected", true);
            }
            else
            {
                _animator.SetBool("playerDetected", false);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(raycastOrigin.transform.position, raycastOrigin.transform.right * detectionRange);
    }
}
