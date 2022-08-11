using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace ClimateManagement
{
    public class TileGenerator : SerializedMonoBehaviour
    {
        [SerializeField] private TileDatabase tileDatabase = default;
        [SerializeField] private float tileWidth = default;
        [SerializeField] private int initialRingCount = default;
        [SerializeField] private int ringCount = default;
        [SerializeField] private int maxStages = default;
        [SerializeField] private int treeClusters = default;

        private List<Tile> tempTiles;
        public List<Tile> allTiles;
        public Dictionary<int, List<Tile>> ringTiles;

        private int currRingIndex;
        private int currStage;

        private void Awake()
        {
            ringTiles = new Dictionary<int, List<Tile>>();
            tempTiles = new List<Tile>();
            allTiles = new List<Tile>();
            TileController.OnStageUpdate += OnStageUpdate;
        }

        private void OnDestroy()
        {
            TileController.OnStageUpdate -= OnStageUpdate;
        }

        public void CreateMap()
        {
            CreateFirstTile();
            CreateFirstRing();

            currRingIndex = 2;
            currStage = 1;

            CreateRings(initialRingCount);

            StartCoroutine(CreateTrees());
        }

        private IEnumerator CreateTrees()
        {
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < treeClusters; i++)
            {
                int r = Utils.GetRandomValue(0, allTiles.Count);
                List<Tile> adjTiles = GetAdjacentTiles(allTiles[r]);
                ReplaceTile(allTiles[r], GetRandomTreePrefab());

                for (int j = 0; j < adjTiles.Count; j++)
                {
                    Tile tile = GetRandomTreePrefab();
                    ReplaceTile(adjTiles[j], tile);
                }
            }
        }

        public Tile GetRandomTreePrefab()
        {
            List<Tile> trees = new List<Tile>();
            trees = tileDatabase.baseTilePrefabs.FindAll(x => x is Tree);

            if (trees.Count > 0)
            {
                int r = Utils.GetRandomValue(0, trees.Count);
                return trees[r];
            }
            return null;
        }

        public Tile GetRandomTree()
        {
            List<Tile> trees = new List<Tile>();
            trees = allTiles.FindAll(x => x is Tree);

            if (trees.Count > 0)
            {
                int r = Utils.GetRandomValue(0, trees.Count);
                return trees[r];
            }
            return null;
        }

        public Tile GetRandomTreeOrEmpty()
        {
            List<Tile> trees = new List<Tile>();
            trees = allTiles.FindAll(x => x is Tree || x is Default);

            if (trees.Count > 0)
            {
                int r = Utils.GetRandomValue(0, trees.Count);
                return trees[r];
            }
            return null;
        }

        public Tile GetRandomHouse()
        {
            List<Tile> houses = new List<Tile>();
            houses = allTiles.FindAll(x => x is House);

            if (houses.Count > 0)
            {
                int r = Utils.GetRandomValue(0, houses.Count);
                return houses[r];
            }
            return null;
        }

        private void OnStageUpdate()
        {
            CreateNextStage();
        }

        public void CreateNextStage()
        {
            if (currStage < maxStages)
            {
                currStage++;
                CreateRings(ringCount);
            }
        }

        public int GetAllTiles()
        {
            int count = 0;
            for (int i = 0; i < allTiles.Count; i++)
            {
                if (allTiles[i] is Default || allTiles[i] is Water)
                    continue;

                count++;
            }
            return count;
        }

        public List<Tile> GetEmptyTiles()
        {
            List<Tile> tiles = new List<Tile>();
            for (int i = 0; i < allTiles.Count; i++)
            {
                if (allTiles[i] is Default)
                {
                    tiles.Add(allTiles[i]);
                }
            }
            return tiles;
        }

        public int GetAllGoodTiles()
        {
            int count = 0;
            for (int i = 0; i < allTiles.Count; i++)
            {
                if (allTiles[i] is IGoodTile)
                    count++;
            }
            return count;
        }

        public int GetAllBadTiles()
        {
            int count = 0;
            for (int i = 0; i < allTiles.Count; i++)
            {
                if (allTiles[i] is IBadTile)
                    count++;
            }
            return count;
        }

        private void CreateRings(int ringCount)
        {
            for (int r = 0; r < ringCount; r++)
            {
                ringTiles.TryGetValue(ringTiles.Count - 1, out List<Tile> prevTiles);
                ringTiles.TryGetValue(ringTiles.Count - 2, out List<Tile> pivotTiles);
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
                        Tile cornerTile = CreateCornerTile(currRingIndex - 1, currTile.transform.position);
                    }

                    pivotTile = allTiles[finalIndex];
                    Vector3 diff = (nextTile.transform.position + currTile.transform.position) * 0.5f;
                    diff.x += diff.x - pivotTile.transform.position.x;
                    diff.z += diff.z - pivotTile.transform.position.z;
                    Tile tile = SpawnTile(diff);

                    if (Mathf.RoundToInt(angle2) % 60 != 0 && currRingIndex > 1)
                    {
                        pos = ++pos % (6 * (currRingIndex - 2));
                        finalIndex = pos + pivotIndex;
                    }
                }

                CacheRing(currRingIndex);
                tempTiles.Clear();
                currRingIndex++;
            }
        }

        private Tile CreateCornerTile(int ringIndex, Vector3 prevTilePos)
        {
            float d = Vector3.Distance(transform.position, prevTilePos) / ringIndex;
            float prevTileAngle = Mathf.Atan2(prevTilePos.z, prevTilePos.x);

            prevTilePos.x += d * Mathf.Cos(prevTileAngle);
            prevTilePos.z += d * Mathf.Sin(prevTileAngle);
            return SpawnTile(prevTilePos);
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
            tempTiles.Clear();
        }

        private void CreateFirstTile()
        {
            Tile initTile = SpawnTile(transform.position);
            CacheRing(0);

            List<Tile> tiles = new List<Tile>();
            tiles.Add(initTile);
            tempTiles.Clear();
        }

        private void CacheRing(int ringId)
        {
            ringTiles.Add(ringId, new List<Tile>(tempTiles));
        }

        private Tile SpawnTile(Vector3 tilePos)
        {
            int rTile = UnityEngine.Random.Range(0, 7);
            Tile tile = Instantiate(tileDatabase.baseTilePrefabs[rTile], transform);
            tile.transform.position = tilePos;
            tempTiles.Add(tile);
            allTiles.Add(tile);
            return tile;
        }

        public void ReplaceTile(Tile tile, Tile replaceTilePrefab)
        {
            tile.gameObject.SetActive(false);
            Tile replaceTile = Instantiate(replaceTilePrefab, transform);
            replaceTile.transform.position = tile.transform.position;

            int index = allTiles.IndexOf(tile);
            allTiles.RemoveAt(index);
            allTiles.Insert(index, replaceTile);
        }

        public List<Tile> GetAdjacentTiles(Tile tile)
        {
            List<Tile> adjTiles = new List<Tile>();

            float angle = 0;
            float offset = (Mathf.PI * 2f) / 6;
            for (int i = 0; i < 6; i++)
            {
                Vector3 tilePos = new Vector3(Mathf.Cos(angle) * tileWidth, 0, Mathf.Sin(angle) * tileWidth);
                Vector3 pos = tile.transform.position;
                pos.y += 0.05f;

                Ray ray = new Ray(pos, tilePos);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.transform.TryGetComponentInParent(out Tile hitTile))
                    {
                        adjTiles.Add(hitTile);
                    }
                }
                angle += offset;
            }
            return adjTiles;
        }

        public List<Tile> GetAdjacentTrees(Tile tile)
        {
            List<Tile> adjTiles = GetAdjacentTiles(tile);

            List<Tile> trees = new List<Tile>();
            for (int i = 0; i < adjTiles.Count; i++)
            {
                if (adjTiles[i] is Tree || adjTiles[i] is Default)
                {
                    trees.Add(adjTiles[i]);
                }
            }
            return trees;
        }

        public List<Tile> GetAdjacentHouses(Tile tile)
        {
            List<Tile> adjTiles = GetAdjacentTiles(tile);

            List<Tile> houses = new List<Tile>();
            for (int i = 0; i < adjTiles.Count; i++)
            {
                if (adjTiles[i] is House)
                {
                    houses.Add(adjTiles[i]);
                }
            }
            return houses;
        }
    }
}