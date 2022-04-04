using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ClimateManagement
{
    public class TileGeneratorV2 : MonoBehaviour
    {
        public List<Tile> tilePrefabs = default;
        public int initialRingCount = default;
        public float tileWidth = default;

        private List<Tile> allTiles;
        private List<Tile> tiles;
        private Dictionary<int, List<Tile>> tileDictionary;

        private int currRingIndex;

        private void Awake()
        {
            tileDictionary = new Dictionary<int, List<Tile>>();
            tiles = new List<Tile>();
            allTiles = new List<Tile>();
        }

        void Start()
        {
            CreateFirstTile();
            CreateFirstRing();
            currRingIndex = 2;
            CreateRings(initialRingCount);

            CreateRings(10);
        }

        [Button("Create Rings", Style = ButtonStyle.Box)]
        private void CreateRings(int ringCount)
        {
            for (int r = 0; r < ringCount; r++)
            {
                tileDictionary.TryGetValue(tileDictionary.Count - 1, out List<Tile> prevTiles);
                tileDictionary.TryGetValue(tileDictionary.Count - 2, out List<Tile> pivotTiles);
                Tile pivotTile = pivotTiles[0];

                int pivotIndex = allTiles.IndexOf(pivotTile);
                int pos = 0;
                int finalIndex = pivotIndex;

                for (int p = 0; p < prevTiles.Count; p++)
                {
                    int q = (p + 1) % prevTiles.Count;

                    Tile currTile = prevTiles[p];
                    Tile nextTile = prevTiles[q];

                    float angle1 = Mathf.Atan2(currTile.transform.position.z, currTile.transform.position.x) * Mathf.Rad2Deg;
                    float angle2 = Mathf.Atan2(nextTile.transform.position.z, nextTile.transform.position.x) * Mathf.Rad2Deg;

                    if (Mathf.RoundToInt(angle1) % 60 == 0)
                    {
                        CreateCornerTile(currRingIndex - 1, currTile.transform.position);
                    }

                    pivotTile = allTiles[finalIndex];
                    Vector3 diff = (nextTile.transform.position + currTile.transform.position) * 0.5f;
                    diff.x += diff.x - pivotTile.transform.position.x;
                    diff.z += diff.z - pivotTile.transform.position.z;
                    SpawnTile(diff);

                    if (Mathf.RoundToInt(angle2) % 60 != 0 && currRingIndex > 1)
                    {
                        pos = ++pos % (6 * (currRingIndex - 2));
                        finalIndex = pos + pivotIndex;
                    }
                }

                CacheRing(currRingIndex);
                tiles.Clear();
                currRingIndex++;
            }
        }

        private void CreateCornerTile(int ringIndex, Vector3 prevTilePos)
        {
            float d = Vector3.Distance(transform.position, prevTilePos) / ringIndex;
            float prevTileAngle = Mathf.Atan2(prevTilePos.z, prevTilePos.x);

            prevTilePos.x += d * Mathf.Cos(prevTileAngle);
            prevTilePos.z += d * Mathf.Sin(prevTileAngle);
            SpawnTile(prevTilePos);
        }

        private void CreateFirstRing()
        {
            float angle = 0f;
            float angleOffset = (Mathf.PI * 2f) / 6;

            for (int i = 0; i < 6; i++)
            {
                Vector3 tilePos = new Vector3(Mathf.Cos(angle) * tileWidth, 0, Mathf.Sin(angle) * tileWidth);
                SpawnTile(tilePos);
                angle += angleOffset;
            }
            CacheRing(1);
            tiles.Clear();
        }

        private void CreateFirstTile()
        {
            SpawnTile(transform.position);
            CacheRing(0);
            tiles.Clear();
        }

        private void CacheRing(int ringId)
        {
            tileDictionary.Add(ringId, new List<Tile>(tiles));
        }

        private void SpawnTile(Vector3 tilePos)
        {
            int rTile = UnityEngine.Random.Range(0, tilePrefabs.Count);
            Tile tile = Instantiate(tilePrefabs[rTile], transform);
            tile.transform.position = tilePos;
            tiles.Add(tile);
            allTiles.Add(tile);
        }
    }
}