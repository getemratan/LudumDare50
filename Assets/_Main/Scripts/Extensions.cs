using System;
using System.Collections;
using UnityEngine;

public static  class Extensions
{
    public static bool TryGetComponentInParent<T>(this Transform transform, out T component)
    {
        component = transform.GetComponentInParent<T>();
        if (component != null)
        {
            return true;
        }
        return false;
    }
}