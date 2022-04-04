using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

namespace ClimateManagement
{
    public class TileController : MonoBehaviour
    {
        public static event System.Action OnStageUpdate;

        [SerializeField] private TileDatabase tileDatabase = default;
        [SerializeField] private TileGenerator tileGenerator = default;
        [SerializeField] private float popupTimer = default;

        private bool hasStarted;
        private float timer;

        private void Awake()
        {
            GameManager.OnGameStart += OnGameStart;
            TileInput.OnTileHover += OnTileHover;
            TileInput.OnTileSelected += OnTileSelected;
            timer = popupTimer;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStart -= OnGameStart;
            TileInput.OnTileHover -= OnTileHover;
            TileInput.OnTileSelected -= OnTileSelected;
        }

        private void Update()
        {
            if (!hasStarted)
                return;

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = popupTimer;
                SpawnPopup();
            }
        }

        private void SpawnPopup()
        {
            Tile tree = tileGenerator.GetRandomTree();
            if (tree == null)
                return;

            List<Tile> houses = tileDatabase.popupTiles.FindAll(x => x is House);
            int r = Utils.GetRandomValue(0, houses.Count);
            tileGenerator.ReplaceTile(tree, houses[r]);
        }

        private void OnGameStart()
        {
            List<Tile> houses = tileDatabase.popupTiles.FindAll(x => x is House);
            int r = Utils.GetRandomValue(0, houses.Count);
            Tile popupTile = houses[r];

            tileGenerator.ringTiles.TryGetValue(0, out List<Tile> tiles);
            Tile initTile = tiles[0];
            tileGenerator.ReplaceTile(initTile, popupTile);

            hasStarted = true;
        }

        private void OnTileSelected(Tile tile)
        {
            if (tile is IReplaceableTile)
            {
                int r = Utils.GetRandomValue(0, tileDatabase.replaceTiles.Count);
                tileGenerator.ReplaceTile(tile, tileDatabase.replaceTiles[r]);
            }
        }

        private void OnTileHover(Tile tile)
        {
        }
    }
}