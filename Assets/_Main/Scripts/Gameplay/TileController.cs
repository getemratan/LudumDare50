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
        public static event Action<TileType> OnTilePlaced;

        [SerializeField] private TileDatabase tileDatabase = default;
        [SerializeField] private TileGenerator tileGenerator = default;
        [SerializeField] private TileListSO tileListSO = default;
        [SerializeField] private float popupTime = default;
        [SerializeField] private float mapTime = default;

        private TileType currentTileType = TileType.Tree;

        public TileType CurrentTileType { get => currentTileType; }

        private bool hasStarted;
        private float popupTimer;
        private float maptimer;

        private void Awake()
        {
            GameManager.OnGameStart += OnGameStart;
            TileInput.OnTileHover += OnTileHover;
            TileInput.OnTileSelected += OnTileSelected;
            PlaceableButton.OnTileTypeSelected += SetNewCurrentTileType;
            popupTimer = popupTime;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStart -= OnGameStart;
            TileInput.OnTileHover -= OnTileHover;
            TileInput.OnTileSelected -= OnTileSelected;
            PlaceableButton.OnTileTypeSelected -= SetNewCurrentTileType;
        }

        private void Update()
        {
            if (!hasStarted)
                return;

            popupTimer -= Time.deltaTime;
            if (popupTimer <= 0)
            {
                popupTimer = popupTime;
                int r = UnityEngine.Random.Range(0, 6);
                SpawnPopup();
            }

            maptimer -= Time.deltaTime;
            if (maptimer <= 0)
            {
                maptimer = mapTime;
                OnStageUpdate?.Invoke();
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
                int r = Utils.GetRandomValue(0, tileListSO.TileTypeLists[currentTileType].Count);
                tileGenerator.ReplaceTile(tile, tileListSO.TileTypeLists[currentTileType][r]);
                OnTilePlaced?.Invoke(currentTileType);
                tileGenerator.GetAdjacentTiles(tile);
            }
        }

        private void SetNewCurrentTileType(TileType newTileType)
        {
            if (currentTileType != newTileType)
            {
                currentTileType = newTileType;
            }
        }

        private void OnTileHover(Tile tile)
        {
        }
    }
}