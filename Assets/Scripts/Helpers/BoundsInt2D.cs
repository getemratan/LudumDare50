using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoundsInt2D 
{
    [SerializeField] public Vector2Int min;
    [SerializeField] public Vector2Int max;

    public BoundsInt2D(Vector2Int _min, Vector2Int _max )
    {
        min = _min;
        max = _max;
    }

    public Vector2Int size { get => max - min; }

    public bool Contains(Vector2Int coords)
    {
        return coords.y >= min.y && coords.y < max.y
            && coords.x >= min.x && coords.x < max.x;
    }
}
