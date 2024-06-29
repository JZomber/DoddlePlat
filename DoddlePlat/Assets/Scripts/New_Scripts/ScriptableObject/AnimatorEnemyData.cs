using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimatorEnemyData", menuName = "Data/AnimatorEnemyData")]
public class AnimatorEnemyData : ScriptableObject
{
    private const string _alive = "isAlive";
    private const string _hit = "isHit";
    private const string _playerDetected = "playerDetected";

    public string s_alive => _alive;
    public string s_hit => _hit;
    public string s_playerDetected => _playerDetected;
}
