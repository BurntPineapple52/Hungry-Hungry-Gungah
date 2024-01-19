using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PieceTile : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    Piece _parentPiece;

    Camera _mainCam;

    bool _connectedToBoard = false;

    int _posXConnected = 0;
    int _posYConnected = 0;

    bool _canBeDraggeed = true;

    [SerializeField]
    SpriteRenderer _renderer;

    public int PosXConnected { get => _posXConnected; }
    public int PosYConnected { get => _posYConnected; }

    public Piece ParentPiece { get => _parentPiece; }

    public bool ConnectedToBoard { get => _connectedToBoard; }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_canBeDraggeed)
        {
            return;
        }

        if (ManagerLocator.Instance.Game.IsPaused() || ManagerLocator.Instance.Game.GameEnded())
        {
            return;
        }

        _parentPiece.TurnOffPointsText();
        _parentPiece.UseNormalScale();
        ManagerLocator.Instance.Sound.GrabPiece();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_canBeDraggeed)
        {
            return;
        }

        if (ManagerLocator.Instance.Game.GameEnded())
        {
            _parentPiece.gameObject.SetActive(false);
            return;
        }

        Vector3 worldPosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        _parentPiece.MovePieceIgnoringLocal(worldPosition, transform.localPosition);
        
        ManagerLocator.Instance.Board.CheckValidPosition(_parentPiece);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_canBeDraggeed)
        {
            return;
        }

        ManagerLocator.Instance.Board.CleanColor();

        if (!_connectedToBoard)
        {
            _parentPiece.ReturnToOriginState();
            return;
        }

        _parentPiece.Paint(true);
        _parentPiece.MovePieceIgnoringLocal(ManagerLocator.Instance.Board.GetBoardTile(_posXConnected, _posYConnected).transform.position, transform.localPosition);
        ManagerLocator.Instance.Game.CalculatePoints(_parentPiece);
        _parentPiece.PlaceOnTheBoard();
        ManagerLocator.Instance.Game.TryAddBonusTile();
    }

    public void SetParentPiece(Piece parentPiece)
    {
        _parentPiece = parentPiece;
    }

    public void ConnectToBoardPosition(int x, int y)
    {
        _connectedToBoard = true;
        _posXConnected = x;
        _posYConnected = y;
    }

    public void DisconnectFormBoard()
    {
        _connectedToBoard = false;
    }

    public void Paint(Color color)
    {
        _renderer.color = color;
    }

    public void TurnOff()
    {
        _renderer.sortingOrder--;
        _canBeDraggeed = false;
    }
}
