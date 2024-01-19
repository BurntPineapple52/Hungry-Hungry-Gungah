using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
struct Column
{
    public Tile[] _rows;
}

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int _width = 5;
    [SerializeField]
    private int _height = 5;

    [Range(0,1)]
    [SerializeField]
    private float _saturation = 0.4f;

    [SerializeField] private float _minDistanceToColorBoard = 1f;

    [SerializeField] private Column[] _boardTiles;

    List<Tile> _freeSpaces = new List<Tile>();

    List<Piece> _piecesOnBoard = new List<Piece>();

    private void Start()
    {
        PrepareFreeSpaceList();
        ManagerLocator.Instance.Stats.UpdateCurrentMultiplier(_freeSpaces.Count);
        ActivateInvalidZones();
    }

    public void AddBoardTileToGrid(Tile tile, int x, int y)
    {
        x = Mathf.Abs(x);
        y = Mathf.Abs(y);

        if (x >= _width)
        {
            Debug.LogError("AddBoardTileToGrid wrong width");
            return;
        }

        if (y >= _height)
        {
            Debug.LogError("AddBoardTileToGrid wrong height");
            return;
        }

        _boardTiles[x]._rows[y] = tile;
    }

    public void SetSize(int width, int height)
    {
        width = Mathf.Abs(width);
        height = Mathf.Abs(height);

        _width = width;
        _height = height;

        _boardTiles = new Column[width];

        for (int i = 0; i < width; i++)
        {
            _boardTiles[i] = new Column();
            _boardTiles[i]._rows = new Tile[height];
        }
    }

    public Tile GetBoardTile(int x, int y)
    {
        x = Mathf.Abs(x);
        y = Mathf.Abs(y);

        if (x >= _width)
        {
            Debug.LogError("GetBoardTile wrong width");
            return null;
        }

        if (y >= _height)
        {
            Debug.LogError("GetBoardTile wrong height");
            return null;
        }

        return _boardTiles[x]._rows[y];
    }

    public bool CheckValidPosition(Piece piece)
    {
        for (int i = 0; i < piece.Widht; i++)
        {
            for (int c = 0; c < piece.Height; c++)
            {
                if (!CheckIfMeetMinDistance(piece.GetTile(i,c)))
                {
                    continue;
                }

                CleanColor();

                if (CheckValidPositionUsingReference(piece.GetTile(i, c), i, c))
                {
                    PaintBoardReactingToPiece(piece, true);
                    piece.Paint(true);
                    return true;
                }

                PaintBoardReactingToPiece(piece, false);
                piece.DisconnectTiles();
                piece.Paint(false);
                return false;
            }
        }

        piece.Paint(true);
        CleanColor();

        return false;
    }

    bool CheckIfMeetMinDistance(PieceTile tile)
    {
        if (tile == null)
        {
            return false;
        }

        float bestDist = -1;
        float dist = 0;

        int x = 0;
        int y = 0;

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                dist = Vector3.Distance(tile.transform.position, _boardTiles[i]._rows[c].transform.position);

                if (dist > _minDistanceToColorBoard)
                {
                    continue;
                }

                if (bestDist < 0 || dist < bestDist)
                {
                    bestDist = dist;
                    x = i;
                    y = c;
                }
            }
        }

        if (bestDist < 0)
        {
            return false;
        }

        tile.ConnectToBoardPosition(x, y);
        return true;
    }

    bool CheckValidPositionUsingReference(PieceTile tile, int xTileInPiece, int yTileInPiece)
    {
        if (tile == null)
        {
            return false;
        }

        bool isValid = true;

        int x = tile.PosXConnected;
        int y = tile.PosYConnected;

        if (_boardTiles[x]._rows[y].IsOccupied())
        {
            tile.DisconnectFormBoard();
            isValid = false;
        }

        Piece piece = tile.ParentPiece;

        for (int i = 0; i < piece.Widht; i++)
        {
            for (int c = 0; c < piece.Height; c++)
            {
                if (i == xTileInPiece && c == yTileInPiece )
                {
                    continue;
                }

                PieceTile tempTile = piece.GetTile(i, c);

                if (tempTile == null)
                {
                    continue;
                }

                int newX = x + i - xTileInPiece;
                int newY = y + c - yTileInPiece;

                if (newX < 0 || newX >= _width)
                {
                    isValid = false;
                    continue;
                }

                if (newY < 0 || newY >= _height)
                {
                    isValid = false;
                    continue;
                }

                if (_boardTiles[newX]._rows[newY].IsOccupied())
                {
                    isValid = false;
                    continue;
                }

                tempTile.ConnectToBoardPosition(newX, newY);
            }
        }

        return isValid;
    }

    void PaintBoardReactingToPiece(Piece piece, bool normalColor)
    {
        Color color;

        if (normalColor)
        {
            color = piece.NormalColor;
        }
        else
        {
            color = piece.WrongColor;
        }

        float colorH;
        float colorS;
        float colorV;

        Color.RGBToHSV(color, out colorH, out colorS, out colorV);
        color = Color.HSVToRGB(colorH, _saturation, colorV);

        for (int i = 0; i < piece.Widht; i++)
        {
            for (int c = 0; c < piece.Height; c++)
            {
                PieceTile temp = piece.GetTile(i, c);

                if (temp == null)
                {
                    continue;
                }

                if (!temp.ConnectedToBoard)
                {
                    continue;
                }

                _boardTiles[temp.PosXConnected]._rows[temp.PosYConnected].SetHighlightColor(color);
            }
        }
    }

    public void CleanColor()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                _boardTiles[i]._rows[c].TurnOffHighlight();
            }
        }
    }

    public void OccupySpace(int x, int y)
    {
        Tile temp = _boardTiles[x]._rows[y];
        temp.SetOccupy();
        _freeSpaces.Remove(temp);
    }

    public bool IsAnySpaceFree()
    {
        if (_freeSpaces.Count > 0)
        {
            return true;
        }

        return false;
    }

    public Tile GetFreeSpace(bool occupySpace)
    {
        if (!IsAnySpaceFree())
        {
            return null;
        }

        Tile temp = _freeSpaces[0];

        if (occupySpace)
        {
            temp.SetOccupy();
            _freeSpaces.RemoveAt(0);
            return temp;
        }

        _freeSpaces.Shuffle();
        return temp;
    }

    public void PrepareFreeSpaceList()
    {
        _freeSpaces.Clear();

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                Tile temp = _boardTiles[c]._rows[i];

                if (!temp.IsOccupied())
                {
                    _freeSpaces.Add(temp);
                }
            }
        }

        _freeSpaces.Shuffle();
    }

    public int GetFreeTilesAtStart()
    {
        int counter = 0;

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                if (!_boardTiles[c]._rows[i].IsInvalidZone())
                {
                    counter++;
                }
            }
        }

        return counter;
    }

    public void AddPieceToBoardList(Piece piece)
    {
        _piecesOnBoard.Add(piece);
    }

    public void RemoveAllPieces()
    {
        for (int i = 0; i < _piecesOnBoard.Count; i++)
        {
            Destroy(_piecesOnBoard[i].gameObject);
        }

        _piecesOnBoard.Clear();
    }

    public void StartSwallow()
    {
        RemoveAllPieces();
        CleanColor();
        ClearTiles();
        PrepareFreeSpaceList();
        GenerateInvalidZones(ManagerLocator.Instance.Game.BlockedTiles);
        ActivateInvalidZones();
        ManagerLocator.Instance.Stats.UpdateCurrentMultiplier(_freeSpaces.Count);
    }

    void GenerateInvalidZones(int num)
    {
        if (num >= _freeSpaces.Count)
        {
            num = _freeSpaces.Count - 1;
        }

        for (int i = 0; i < num; i++)
        {
            _freeSpaces[0].SetTemporalInvalid();
            _freeSpaces.RemoveAt(0);
        }
    }

    void ClearTiles()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                _boardTiles[i]._rows[c].ClearTile();
            }
        }
    }

    void ActivateInvalidZones()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                _boardTiles[i]._rows[c].SetColorInvalid();
            }
        }
    }
}
