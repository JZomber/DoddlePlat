using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private FactoryInitializer enemyFactoryInitializer;

    [SerializeField] private Transform[] batSpawns;
    [SerializeField] private Transform[] plantSpawns;
    [SerializeField] private Transform rinoSpawn;
    
    void Start()
    {
        foreach (var spawns in batSpawns)
        {
            var newEnemy = enemyFactoryInitializer.GetEnemy(EnemyFactory.BAT_ENEMY);
            Instantiate(newEnemy, spawns);
        }

        foreach (var spawns in plantSpawns)
        {
            var newEnemy = enemyFactoryInitializer.GetEnemy(EnemyFactory.PLANT_ENEMY);
            Instantiate(newEnemy, spawns);
        }
        
        var enemy = enemyFactoryInitializer.GetEnemy(EnemyFactory.RINO_ENEMY);
        Instantiate(enemy, rinoSpawn);
    }
}
