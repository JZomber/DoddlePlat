using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : StaticEnemy
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(animatorData.s_playerDetected, true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool(animatorData.s_playerDetected, false);
        }
    }
}
