using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private bool isHex = default;
    [SerializeField] private int rows = default;
    [SerializeField] private int columns = default;
    [SerializeField] private Vector2 tileSize = default;
    [SerializeField] private List<Tile> defaultTilePrefabs = default;
    [SerializeField] private LevelGrid levelGrid = default;

    private Vector2 gridOffset;

    private void Start()
    {
        CalcOffset();
        Generate();
    }

    private void CalcOffset()
    {
        if (isHex)
        {
            float offset = 0;
            if (columns / 2 % 2 != 0)
                offset = tileSize.x / 2f;

            float x = -tileSize.x * (rows / 2f) - offset;
            float y = tileSize.y * 0.75f * (columns / 2f);
            gridOffset = new Vector2(x, y);
        }
        else
        {
            float x = -tileSize.x * ((float)rows / 2f);
            float y = tileSize.y * ((float)columns / 2f);
            gridOffset = new Vector2(x, y);
        }
    }

    private void Generate()
    {
        int tileId = 0;
        for (int c = 0; c < columns; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Vector3 pos = new Vector3(r, 0, c);
                int rTileIndex = Random.Range(0, defaultTilePrefabs.Count - 1);
                Tile tile = Instantiate(defaultTilePrefabs[rTileIndex], transform);
                tile.tileId = tileId;
                tile.transform.position = isHex ? GetHexGridPos(pos) : GetGridPos(pos);
                tile.name = $"{r} X {c}";
                levelGrid.AddTile(tile);
                tileId++;
            }
        }
    }

    private Vector3 GetHexGridPos(Vector3 pos)
    {
        float spacing = 0;

        if (pos.z % 2 != 0)
            spacing = tileSize.x / 2f;

        float x = gridOffset.x + pos.x * tileSize.x + spacing;
        float z = gridOffset.y - pos.z * tileSize.y * 0.75f;

        return new Vector3(x, 0, z);
    }

    private Vector3 GetGridPos(Vector3 pos)
    {
        float x = gridOffset.x + pos.x * tileSize.x;
        float z = gridOffset.y - pos.z * tileSize.y;

        return new Vector3(x, 0, z);
    }
}