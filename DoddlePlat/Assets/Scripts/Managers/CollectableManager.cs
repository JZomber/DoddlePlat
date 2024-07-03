using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private List<CollectablePool> collectablePools;
    private List<GameObject> objPool;

    public event Action OnAllFruitsCollected;

    private int fruitCounter = 0;
    
    void Start()
    {
        objPool = new List<GameObject>();

        foreach (var poolItem in collectablePools)
        {
            for (int i = 0; i < poolItem.amount; i++)
            {
                GameObject newCollectable = Instantiate(poolItem.collectablePrefab);

                if (poolItem.positions != null && poolItem.positions.Length > 0)
                {
                    newCollectable.transform.position = poolItem.positions[i].position;
                }
                
                newCollectable.SetActive(true);
                objPool.Add(newCollectable);

                Collectable collectableScript = newCollectable.GetComponent<Collectable>();

                if (collectableScript != null)
                {
                    collectableScript.OnFruitCollected += HandlerObjectCollected;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandlerObjectCollected()
    {
        fruitCounter++;
        
        if (fruitCounter >= 5)
        {
            OnAllFruitsCollected?.Invoke();
            fruitCounter = 0;
        }
    }

    private void OnDisable()
    {
        if (objPool != null)
        {
            foreach (GameObject fruits in objPool)
            {
                if (fruits != null)
                {
                    Collectable collectableScript = fruits.GetComponent<Collectable>();

                    if (collectableScript != null)
                    {
                        collectableScript.OnFruitCollected -= HandlerObjectCollected;
                    }
                }
            }
        }
    }
}

[Serializable]
public class CollectablePool
{
    public Zones zone;
    public GameObject collectablePrefab;
    public int amount;
    public Transform[] positions;
}

public enum Zones
{
    Zone1,
    Zone2,
    Zone3
}
