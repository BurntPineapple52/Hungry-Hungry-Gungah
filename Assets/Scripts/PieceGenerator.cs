using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour
{
    [SerializeField]
    ArrayLayout _pieceDisplay;

    [SerializeField]
    int _points;

    [SerializeField]
    Color _normalColor;

    [SerializeField]
    Piece _piecePrefab;

    [SerializeField]
    PieceTile _pieceTilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePiece();
    }

    void GeneratePiece()
    {
        var piece = Instantiate(_piecePrefab);
        piece.transform.position = Vector3.zero;
        piece.SetSize(_pieceDisplay.rows[0].row.Length, _pieceDisplay.rows.Length);
        piece.SetPoints(_points);
        piece.SetNormalColor(_normalColor);

        for (int x = 0; x < _pieceDisplay.rows.Length; x++)
        {
            for (int y = 0; y < _pieceDisplay.rows[x].row.Length; y++)
            {
                PieceTile spawnedTile = null;

                if (_pieceDisplay.rows[x].row[y])
                {
                    spawnedTile = Instantiate(_pieceTilePrefab, new Vector3(y, _pieceDisplay.rows.Length - 1 - x), Quaternion.identity, piece.transform);
                    spawnedTile.name = $"PieceTile {y} { _pieceDisplay.rows.Length - 1 - x}";
                    spawnedTile.SetParentPiece(piece);
                }

                piece.AddTileToPiece(spawnedTile, y, _pieceDisplay.rows.Length - 1 - x);
            }
        }

        piece.Paint(true);
    }
}
