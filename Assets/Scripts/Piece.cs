using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
struct PieceColumn
{
    public PieceTile[] _rows;
}

public class Piece : MonoBehaviour
{
    [SerializeField]
    private int _points = 2;

    [SerializeField]
    private TMP_Text _pointsText;

    [SerializeField]
    private int _width = 5;
    [SerializeField]
    private int _height = 5;

    [SerializeField] Color _normalColor;
    [SerializeField] Color _wrongColor;

    [SerializeField] private PieceColumn[] _pieceTiles;

    Vector3 _originalPos;
    Vector3 _waitingScale;
    Vector3 _normalScale;
    Vector3 _originalPosWithOffset;

    public int Widht { get => _width; }
    public int Height { get => _height; }

    public Color NormalColor { get => _normalColor; }
    public Color WrongColor { get => _wrongColor; }

    public int Points { get => _points; }

    private void Start()
    {
        _pointsText.text = _points.ToString();
    }

    public void AddTileToPiece(PieceTile tile, int x, int y)
    {
        x = Mathf.Abs(x);
        y = Mathf.Abs(y);

        if (x >= _width)
        {
            Debug.LogError("AddTileToPiece wrong width");
            return;
        }

        if (y >= _height)
        {
            Debug.LogError("AddTileToPiece wrong height");
            return;
        }

        _pieceTiles[x]._rows[y] = tile;
    }

    public void SetSize(int width, int height)
    {
        width = Mathf.Abs(width);
        height = Mathf.Abs(height);

        _width = width;
        _height = height;

        _pieceTiles = new PieceColumn[width];

        for (int i = 0; i < width; i++)
        {
            _pieceTiles[i] = new PieceColumn();
            _pieceTiles[i]._rows = new PieceTile[height];
        }
    }

    public PieceTile GetTile(int x, int y)
    {
        if (x >= _width)
        {
            return null;
        }

        if (y >= _height)
        {
            return null;
        }

        return _pieceTiles[x]._rows[y];
    }

    public void MovePieceIgnoringLocal(Vector3 position, Vector3 localPosition)
    {
        position.z = transform.position.z;
        transform.position = position - localPosition;
    }

    public void DisconnectTiles()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                if (_pieceTiles[i]._rows[c] != null)
                {
                    _pieceTiles[i]._rows[c].DisconnectFormBoard();
                }
            }
        }
    }

    public void Paint(bool normalColor)
    {
        Color temp;

        if (!normalColor)
        {
            temp = _wrongColor;
        }
        else
        {
            temp = _normalColor;
        }

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                if (_pieceTiles[i]._rows[c] != null)
                {
                    _pieceTiles[i]._rows[c].Paint(temp);
                }
            }
        }
    }

    public void SetStartingPosAndScale(Vector3 pos, Vector3 scale)
    {
        _originalPos = pos;
        _waitingScale = scale;
        _originalPosWithOffset = pos;
        float num = scale.x;

        num *= (_width - 1);
        num /= 2;

        _originalPosWithOffset.x -= num;
        _originalPosWithOffset.y -= num;
    }

    public void SetNormalScale(Vector3 scale)
    {
        _normalScale = scale;
    }

    public void ReturnToOriginState()
    {
        transform.localScale = _waitingScale;
        transform.position = _originalPosWithOffset;
        Paint(true);
        TurnOnPointsText();
    }

    public void UseNormalScale()
    {
        transform.localScale = _normalScale;
    }

    public void PlaceOnTheBoard()
    {
        _pointsText.transform.parent.gameObject.SetActive(false);

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                PieceTile temp = _pieceTiles[i]._rows[c];

                if (temp == null)
                {
                    continue;
                }

                temp.TurnOff();
                ManagerLocator.Instance.Board.OccupySpace(temp.PosXConnected, temp.PosYConnected);
                ManagerLocator.Instance.Board.AddPieceToBoardList(this);
            }
        }

        ManagerLocator.Instance.Game.GetNextPiece(_originalPos);
    }

    public bool HasConnectedTile(Tile tile)
    {
        PieceTile temp;

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                temp = _pieceTiles[i]._rows[c];

                if (temp == null)
                {
                    continue;
                }

                if (tile == ManagerLocator.Instance.Board.GetBoardTile(temp.PosXConnected, temp.PosYConnected))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetTilesCount()
    {
        int counter = 0;

        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                if (_pieceTiles[i]._rows[c] != null)
                {
                    counter++;
                }
            }
        }

        return counter;
    }

    public void TurnOffPointsText()
    {
        _pointsText.gameObject.SetActive(false);
    }

    public void TurnOnPointsText()
    {
        _pointsText.gameObject.SetActive(true);
    }

    public void SetPoints(int points)
    {
        _points = points;
        _pointsText.text = points.ToString();
    }

    public void SetNormalColor(Color color)
    {
        _normalColor = color;
    }
}
