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
        [SerializeField] private TileListSO tileListSO = default; 

        private TileType currentTileType;

        private void Awake()
        {
            TileInput.OnTileHover += OnTileHover;
            TileInput.OnTileSelected += OnTileSelected;
            PlaceableButton.OnTileTypeSelected += SetNewCurrentTileType;
        }

        private void OnDestroy()
        {
            TileInput.OnTileHover -= OnTileHover;
            TileInput.OnTileSelected -= OnTileSelected;
            PlaceableButton.OnTileTypeSelected -= SetNewCurrentTileType;
        }

        private void SetNewCurrentTileType(TileType newTileType)
        {
            if (currentTileType != newTileType)
            {
                currentTileType = newTileType;
            }
        }

        private void OnTileSelected(Tile tile)
        {
            int rTile = UnityEngine.Random.Range(0, tileListSO.TileTypeLists[currentTileType].Count);
            Tile replaceTile = Instantiate(tileListSO.TileTypeLists[currentTileType][rTile]);
            levelGrid.ReplaceTile(tile, replaceTile);
        }

        private void OnTileHover(Tile tile)
        {
        }
    }
}
