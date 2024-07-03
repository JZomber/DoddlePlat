using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : AbstractFactory<EnemyClass>
{
    public const string BAT_ENEMY = "Bat";
    public const string PLANT_ENEMY = "Plant";
    public const string RINO_ENEMY = "Rino";

    public bool Initialized { get; private set; }
    
    private EnemyClass batPrefab;
    private EnemyClass plantPrefab;
    private EnemyClass rinoPrefab;

    public override EnemyClass CreateProduct(string productCode)
    {
        if (productCode == BAT_ENEMY)
        {
            return batPrefab;
        }
        if (productCode == PLANT_ENEMY)
        {
            return plantPrefab;
        }
        if (productCode == RINO_ENEMY)
        {
            return rinoPrefab;
        }
        
        return default;
    }

    public void Initialize(EnemyClass batPrefab, EnemyClass plantPrefab, EnemyClass rinoPrefab)
    {
        this.batPrefab = batPrefab;
        this.plantPrefab = plantPrefab;
        this.rinoPrefab = rinoPrefab;
        
        Initialized = true;
    }
}
