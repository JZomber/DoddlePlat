using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilExtension
{
    public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component) where T : class
    {
        component = gameObject.GetComponentInParent<T>();
        return component != null;
    }
}
