using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimateManagement
{
	public class TileController : MonoBehaviour
	{
        [SerializeField] private List<Tile> replaceTilePrefabs = default;
        [SerializeField] private LevelGrid levelGrid = default;

        private void Awake()
        {
            TileInput.OnTileHover += OnTileHover;
            TileInput.OnTileSelected += OnTileSelected;
        }

        private void OnDestroy()
        {
            TileInput.OnTileHover -= OnTileHover;
            TileInput.OnTileSelected -= OnTileSelected;
        }

        private void OnTileSelected(Tile tile)
        {
            int rTile = UnityEngine.Random.Range(0, replaceTilePrefabs.Count);
            Tile replaceTile = Instantiate(replaceTilePrefabs[rTile]);
            levelGrid.ReplaceTile(tile, replaceTile);
        }

        private void OnTileHover(Tile tile)
        {
        }
    }
}
