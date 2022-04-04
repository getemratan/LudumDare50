using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TileList", menuName = "Tile/TileList")]
public class TileListSO : SerializedScriptableObject
{
    public Dictionary<TileType, TileTypeList> TileTypeLists;
}

[Serializable]
public class TileTypeList
{
    public TileType TileType;
    public List<Tile> Tiles;
}