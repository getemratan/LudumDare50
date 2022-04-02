using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public List<Tile> tiles = default;

    private void Awake()
    {
        tiles = new List<Tile>();
    }

    public void AddTile(Tile tile)
    {
        tiles.Add(tile);
    }

    public void ReplaceTile(Tile tile, Tile replaceTile)
    {
        int tileIndex = tiles.IndexOf(tile);
        tiles.RemoveAt(tileIndex);
        tiles.Insert(tileIndex, replaceTile);

        replaceTile.transform.position = tile.transform.position;
        replaceTile.transform.SetParent(transform);

        Destroy(tile.gameObject);
    }
}
