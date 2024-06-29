using UnityEngine;

public struct EnemyStats
{
    [Header("Enemy Stats")]
    int _lives;
    
    [Header("Enemy Colliders")]
    Collider2D _enemyCollider;
    Collider2D _weakEnemyCollider;

    [Header("Animation")]
    Animator _animator;

    const string s_IsAlive = "isAlive";
    const string s_IsHIt = "isHit";
    const string s_PlayerDetected = "playerDetected";

    bool _isAlive;
    bool _isHit;
    bool _playerDetected;
}
