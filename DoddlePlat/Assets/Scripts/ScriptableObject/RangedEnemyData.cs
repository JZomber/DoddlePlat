using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedEnemyData", menuName = "Data/RangedEnemyData")]
public class RangedEnemyData : ScriptableObject
{
    [Header("Detection & Attack")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectionRange;
    [SerializeField] private GameObject _bulletPrefab;

    public LayerMask playerLayer => _playerLayer;
    public float detectionRange => _detectionRange; 
    public GameObject bulletPrefab => _bulletPrefab;
}
