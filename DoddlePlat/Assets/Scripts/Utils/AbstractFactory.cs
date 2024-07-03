using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractFactory<T> where T : IProduct
{
    private T productType;
    
    public abstract EnemyClass CreateProduct(string productCode);
    
}
public interface IProduct
{
}
