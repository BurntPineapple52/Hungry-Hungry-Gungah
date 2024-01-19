using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileState
{
    Free,
    Occupied,
    Invalid,
    TemporalInvalid
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite _baseSprite, _invalidSprite;
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _highlight;

    [SerializeField] private TileState _state = TileState.Free;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    public void SetHighlightColor(Color color)
    {
        _highlight.gameObject.SetActive(true);
        _highlight.color = color;
    }

    public void TurnOffHighlight()
    {
        _highlight.gameObject.SetActive(false);
    }

    public bool IsOccupied()
    {
        if (_state == TileState.Free)
        {
            return false;
        }

        return true;
    }

    public bool IsInvalidZone()
    {
        if (_state == TileState.Invalid || _state == TileState.TemporalInvalid)
        {
            return true;
        }

        return false;
    }

    public void SetOccupy()
    {
        _state = TileState.Occupied;
    }

    public void ClearTile()
    {
        if (_state == TileState.Invalid)
        {
            return;
        }

        _state = TileState.Free;
        _renderer.sprite = _baseSprite;
    }

    public void SetTemporalInvalid()
    {
        _state = TileState.TemporalInvalid;
    }

    public void SetColorInvalid()
    {
        if (_state != TileState.Invalid && _state != TileState.TemporalInvalid)
        {
            return;
        }

        _renderer.sprite = _invalidSprite;
    }
}
