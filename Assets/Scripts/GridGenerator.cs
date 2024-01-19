using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Range(5, 10)]
    [SerializeField] private int _width = 5;
    [Range(5, 10)]
    [SerializeField] private int _height = 5;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private GridManager _gridManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        var board = Instantiate(_gridManagerPrefab);

        board.SetSize(_width, _height);

        board.transform.position = Vector3.zero;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, board.transform);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                board.AddBoardTileToGrid(spawnedTile, x, y);
            }
        }
    }
}
