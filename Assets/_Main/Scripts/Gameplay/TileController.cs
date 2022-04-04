using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ClimateManagement
{
	public class TileController : MonoBehaviour
	{
        public static event System.Action OnStageUpdate;

        [SerializeField] private TileDatabase tileDatabase = default;
        [SerializeField] private TileGenerator tileGenerator = default;

        private void Awake()
        {
            GameManager.OnGameStart += OnGameStart;
            TileInput.OnTileHover += OnTileHover;
            TileInput.OnTileSelected += OnTileSelected;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStart -= OnGameStart;
            TileInput.OnTileHover -= OnTileHover;
            TileInput.OnTileSelected -= OnTileSelected;
        }

        private void OnGameStart()
        {
            int r = Utils.GetRandomValue(0, tileDatabase.popupTiles.Count);
            Tile popupTile = tileDatabase.popupTiles[r];

            tileGenerator.tileDictionary.TryGetValue(0, out List<Tile> tiles);
            Tile initTile = tiles[0];
            ReplaceTile(initTile, popupTile);
        }

        private void OnTileSelected(Tile tile)
        {
            if(tile is IReplaceableTile)
            {
                int r = Utils.GetRandomValue(0, tileDatabase.replaceTiles.Count);
                ReplaceTile(tile, tileDatabase.replaceTiles[r]);

                OnStageUpdate?.Invoke();
            }
        }

        private void OnTileHover(Tile tile)
        {
        }

        private void ReplaceTile(Tile tile, Tile replaceTilePrefab)
        {
            tile.gameObject.SetActive(false);
            Tile replaceTile = Instantiate(replaceTilePrefab, transform);
            replaceTile.transform.position = tile.transform.position;
        }
    }
}
