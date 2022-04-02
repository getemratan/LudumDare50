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
        
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.TryGetComponent(out Tile tile))
                {
                    HandleTileHover(tile);
                    HandleTileSelection(tile);
                }
            }
            else
            {
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
                OnTileHover?.Invoke(tile);
            }
            else
            {
                if (currTile != prevTile)
                {
                    prevTile = currTile;
                    OnTileHover?.Invoke(tile);
                }
            }
        }
    }
}