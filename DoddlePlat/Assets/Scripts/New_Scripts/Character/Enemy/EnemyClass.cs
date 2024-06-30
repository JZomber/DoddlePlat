using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected int lives;
    
    [Header("Enemy Colliders")]
    [SerializeField] protected Collider2D enemyCollider;
    [SerializeField] protected Collider2D weakEnemyCollider;
    
    [Header("Animation")] 
    [SerializeField] protected AnimatorEnemyData animatorData;
    [SerializeField] protected Animator animator;
    
    protected bool _isAlive = true;
    protected bool _isPlayerDetected;
    
    protected virtual void Start()
    {
        animator.SetBool(animatorData.s_alive, _isAlive);
    }
}

public class MoveAbleEnemy : EnemyClass
{
}

public class RangedEnemy : EnemyClass
{
    [Header("Detection & Attack")] 
    [SerializeField] protected RangedEnemyData rangedData;
    [SerializeField] protected GameObject raycastOrigin;
    [SerializeField] protected Transform shootOrigin;

    protected virtual void Shoot()
    {
    }
}

public class StaticEnemy : MonoBehaviour
{
}
