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
        public static event Action OnTileExit;

        private Tile prevTile;
        private Tile currTile;

        private bool isPaused;

        public bool gameIsPaused { get => isPaused; }

        private void OnEnable()
        {
            ScreensManager.IsPaused += (bool val) => isPaused = val;
        }

        private void OnDisable()
        {
            ScreensManager.IsPaused += (bool val) => isPaused = val;
        }

        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            //Debug.Log("UI: " + results.Count);
            return results.Count > 0;
        }

        private void Update()
        {
            Vector3 dir = new Vector3(Input.mousePosition.x + 
                Camera.main.transform.rotation.eulerAngles.x, Input.mousePosition.y + 45f, Input.mousePosition.z);
            Ray ray = Camera.main.ScreenPointToRay(dir);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && !isPaused && !IsPointerOverUIObject())
            {
                Tile tile;
                if (hit.transform.TryGetComponent(out tile) || hit.transform.TryGetComponentInParent(out tile))
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
                OnTileExit?.Invoke();
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
            //Debug.Log("Hver");
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
            if (tile.ReplaceableTilesTypes.Count > 0 && tile.ReplaceableTilesTypes.Contains(tileController
                .CurrentTileType) && tileController.CurrentTileCount() > 0)
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