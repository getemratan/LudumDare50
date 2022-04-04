using System;
using UnityEngine;

namespace ClimateManagement
{
    public class TileInput : MonoBehaviour
    {
        public static event Action<Tile> OnTileSelected;
        public static event Action<Tile> OnTileHover;

        private Tile prevTile;
        private Tile currTile;

        private TileType currentTileType = TileType.None;

        private void Awake()
        {
            PlaceableButton.OnTileTypeSelected += SetNewCurrentTileType;
        }

        private void OnDestroy()
        {
            PlaceableButton.OnTileTypeSelected -= SetNewCurrentTileType;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Tile tile;
                if (hit.transform.TryGetComponent(out tile) || hit.transform.TryGetComponentInParent(out tile))
                {
                    //HandleTileHover(tile);
                    HandleTileSelection(tile);
                }
            }
            else
            {
                //CursorManager.Instance.SetActiveCursorType(CursorType.Arrow);
                currTile = null;
                prevTile = null;
            }
        }

        private void HandleTileSelection(Tile tile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnTileSelected?.Invoke(tile);
            }
        }

        private void HandleTileHover(Tile tile)
        {
            currTile = tile;
            if (prevTile == null)
            {
                prevTile = currTile;
                HandleCursorType(tile);
                OnTileHover?.Invoke(tile);
            }
            else
            {
                if (currTile != prevTile)
                {
                    prevTile = currTile;
                    HandleCursorType(tile);
                    OnTileHover?.Invoke(tile);
                }
            }
        }

        private void HandleCursorType(Tile tile)
        {
            if (tile.ReplaceableTilesTypes.Count > 0 && tile.ReplaceableTilesTypes.Contains(currentTileType))
            {
                //CursorManager.Instance.SetActiveCursorType(CursorType.Replace);
            }
            else
            {
                //CursorManager.Instance.SetActiveCursorType(CursorType.CantSelect);
            }
        }

        private void SetNewCurrentTileType(TileType newTileType)
        {
            if (currentTileType != newTileType)
            {
                currentTileType = newTileType;
            }
        }
    }
}