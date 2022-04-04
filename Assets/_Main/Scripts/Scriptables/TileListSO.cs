using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
    [CreateAssetMenu(fileName = "TileList", menuName = "Tile/TileList")]
    public class TileListSO : ScriptableObject
    {
        public TileTypeTileListDictionary TileTypeLists;
    }

    [Serializable]
    public class TileTypeList
    {
        public TileType TileType;
        public List<Tile> Tiles;
    }
}
