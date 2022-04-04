using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace ClimateManagement
{
    public class TileInput : MonoBehaviour
    {
        [SerializeField] private TileController tileController = default;

        public static event Action<Tile> OnTileSelected;
        public static event Action<Tile> OnTileHover;

        private Tile prevTile;
        private Tile currTile;

        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Tile tile;
                if (hit.transform.TryGetComponent(out tile) || hit.transform.TryGetComponentInParent(out tile) || !IsPointerOverUIObject())
                {
                    HandleTileHover(tile);
                    HandleTileSelection(tile);
                }
            }
            else
            {
                CursorManager.Instance.SetActiveCursorType(CursorType.Arrow);
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
            if (tile.ReplaceableTilesTypes.Count > 0 && tile.ReplaceableTilesTypes.Contains(tileController.CurrentTileType))
            {
                CursorManager.Instance.SetActiveCursorType(CursorType.Replace);
            }
            else
            {
                CursorManager.Instance.SetActiveCursorType(CursorType.CantSelect);
            }
        }
    }
}