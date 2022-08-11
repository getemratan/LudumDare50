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
        public static event Action OnTilePopUpComplete;
        public static event Action<int> OnAdjacentHouseCalculated;
        public static event Action<int> OnCurrentTileCountUpdated;

        //public delegate void Placed(TileType tileType, int val = 0);
        //public static event Placed OnTilePlaced;

        [SerializeField] private TileDatabase tileDatabase = default;
        [SerializeField] private TileGenerator tileGenerator = default;
        [SerializeField] private TileListSO tileListSO = default;
        [SerializeField] private float popupTime = default;
        [SerializeField] private float mapTime = default;
        [SerializeField] private List<PlaceableButton> placeableButtons = default;
        [SerializeField] private ScreensManager screensManager = default;
        [SerializeField] private TileInput tileInput = default;
        [SerializeField] private EfficiencyBar efficiencyBar = default;
        //[SerializeField] private Highlights highlights = default;

        private TileType currentTileType = TileType.Tree;

        public TileType CurrentTileType { get => currentTileType; }

        private bool hasStarted;
        private float popupTimer;
        private float maptimer;

        private void Awake()
        {
            ScreensManager.OnGameStart += OnGameStart;
            TileInput.OnTileHover += OnTileHover;
            TileInput.OnTileSelected += OnTileSelected;
            PlaceableButton.OnTileTypeSelected += SetNewCurrentTileType;
            popupTimer = popupTime;
        }

        private void OnDestroy()
        {
            ScreensManager.OnGameStart -= OnGameStart;
            TileInput.OnTileHover -= OnTileHover;
            TileInput.OnTileSelected -= OnTileSelected;
            PlaceableButton.OnTileTypeSelected -= SetNewCurrentTileType;
        }

        private void Update()
        {
            if (!hasStarted)
                return;

            popupTimer -= Time.deltaTime;
            if (popupTimer <= 0 && !tileInput.gameIsPaused)
            {
                popupTimer = popupTime;
                int r = UnityEngine.Random.Range(0, 6);
                SpawnPopup();
                OnTilePopUpComplete?.Invoke();
            }
            //maptimer -= Time.deltaTime;
            //if (maptimer <= 0)
            //{
            //    maptimer = mapTime;
            //    OnStageUpdate?.Invoke();
            //}
        }

        private void SpawnPopup()
        {
            Tile tree = tileGenerator.GetRandomTreeOrEmpty();
            if (tree == null)
            {
                screensManager.GameOver();
                return;
            }

            List<Tile> houses = tileDatabase.popupTiles.FindAll(x => x is IPopupable);
            int r = Utils.GetRandomValue(0, houses.Count);
            tileGenerator.ReplaceTile(tree, houses[r]);

            List<Tile> adjTrees = tileGenerator.GetAdjacentTrees(tree);
            //Debug.Log("Trees: " + adjTrees.Count);
            int randSurroundingTilesCount = Utils.GetRandomValue(0, adjTrees.Count + 1);
            //Debug.Log("Surr: " + randSurroundingTilesCount);
            for (int i = 0; i < randSurroundingTilesCount; i++)
            {
                int n = Utils.GetRandomValue(0, houses.Count);
                tileGenerator.ReplaceTile(adjTrees[i], houses[n]);
            }
        }

        private bool IsReplaceValid(Tile tile, Tile replaceTilePrefab)
        {
            bool isValid = false;
            if (tile is Default && replaceTilePrefab is Tree)
            {
                isValid = true;
            }
            else if (tile is Waste && replaceTilePrefab is Default)
            {
                isValid = true;
            }
            else if (tile is Mine && replaceTilePrefab is WindMill)
            {
                isValid = true;
            }

            PlaceableButton placeableButton = placeableButtons.Find(x => x.TileType == currentTileType);
            if (placeableButton.currAmount <= 0)
                isValid = false;

            return isValid;
        }

        private void OnGameStart()
        {
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            tileGenerator.CreateMap();

            yield return new WaitForSeconds(1f);

            //List<Tile> houses = tileDatabase.popupTiles.FindAll(x => x is House);
            //int r = Utils.GetRandomValue(0, houses.Count);
            //Tile popupTile = houses[r];

            //tileGenerator.ringTiles.TryGetValue(0, out List<Tile> tiles);
            //Tile initTile = tiles[0];
            //tileGenerator.ReplaceTile(initTile, popupTile);

            hasStarted = true;
        }

        private void OnTileSelected(Tile tile)
        {
            if (tile is IReplaceableTile)
            {
                int r = Utils.GetRandomValue(0, tileListSO.TileTypeLists[currentTileType].Count);

                if (IsReplaceValid(tile, tileListSO.TileTypeLists[currentTileType][r]))
                {
                    tileGenerator.ReplaceTile(tile, tileListSO.TileTypeLists[currentTileType][r]);
                    OnTilePlaced?.Invoke(currentTileType);
                    OnAdjacentHouseCalculated?.Invoke(tileGenerator.GetAdjacentHouses(tile).Count);
                    //foreach (var adjTile in tileGenerator.GetAdjacentHouses(tile))
                    //{
                    //    highlights.InstantiatePlus(adjTile.transform.position);
                    //}
                }
            }
        }

        private void SetNewCurrentTileType(TileType newTileType)
        {
            if (currentTileType != newTileType)
            {
                currentTileType = newTileType;
                //currentTileCount = currAmount;
            }
        }

        private void OnTileHover(Tile tile)
        {
            //Debug.Log("Adj Houses: " + tileGenerator.GetAdjacentHouses(tile).Count);
            if (CursorManager.Instance.CurrCursorType == CursorType.Replace)
            {
                efficiencyBar.UpdateSlider(tileGenerator.GetAdjacentHouses(tile).Count);
            }    
        }

        public int CurrentTileCount()
        {
            foreach (var button in placeableButtons)
            {
                if (currentTileType == button.TileType)
                {
                    return button.currAmount;
                }
            }

            return 0;
        }
    }
}