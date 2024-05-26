    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerStomp"))
        {
            if (other.GetContact(0).normal.y <= -0.9)
            {
                animator.SetTrigger("isHit");
                other.gameObject.GetComponent<PlayerMovement>().Bounce();
            }
        }
    }
}
