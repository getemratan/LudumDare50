using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClimateManagement
{
	[CreateAssetMenu(menuName = "Create Tile Database", fileName = "Tile Database")]
	public class TileDatabase : ScriptableObject
	{
		public List<Tile> baseTilePrefabs = default;
		public List<Tile> replaceTiles = default;
		public List<Tile> popupTiles = default;
	}
}