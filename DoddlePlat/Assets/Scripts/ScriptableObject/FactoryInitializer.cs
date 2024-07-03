using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFactoryInitializer", menuName = "Factory/EnemyInitializer")]
public class FactoryInitializer : ScriptableObject
{
    private EnemyFactory _enemyFactory = new EnemyFactory();
    [SerializeField] private Bat batPrefab;
    [SerializeField] private Plant plantPrefab;
    [SerializeField] private Rino rinoPrefab;
    

    public EnemyClass GetEnemy(string enemyCode)
    {
        if (!_enemyFactory.Initialized)
        {
            _enemyFactory.Initialize(batPrefab, plantPrefab, rinoPrefab);
        }

        return _enemyFactory.CreateProduct(enemyCode);
    }
}
