using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject _mouth1;

    [SerializeField]
    GameObject _mouth2;

    [SerializeField]
    float _gameTime = 100f;

    [SerializeField]
    float _runningOutTime = 15f;

    bool _isRunningOutOfTime = false;

    [SerializeField]
    BonusTile _bonusTile;

    [Range(0, 100)]
    [SerializeField]
    float _bonusTileAppAppearanceChance = 20;

    [SerializeField]
    Vector3 _nextPieceScale = new Vector3(1, 1, 1);

    [SerializeField]
    Vector3 _normalPieceScale = new Vector3(1,1,1);

    [SerializeField]
    Transform _hiddenPiecesPosition;

    [SerializeField]
    Transform[] _nextPiecePositions;

    [SerializeField]
    BagOfPieces _bagOfPieces;

    List<Piece> _nextPieces = new List<Piece>();

    bool _bonusOnBoard = false;

    bool _timerPaused = false;

    Tile _currentBonusTileBoardSpace = null;
    BonusTile _currentBonusTile = null;

    List<BonusTile> _bonusTilePool = new List<BonusTile>();
    int _amountOfBonusPool = 20;

    bool _endGame = false;

    int _blockedTiles = 0;

    public int BlockedTiles { get => _blockedTiles; }

    private void Start()
    {
        ManagerLocator.Instance.UI.UpdateTimeLeft(_gameTime);

        GenerateBonusTilePool();
        GenerateBag();

        for (int i = 0; i < _nextPiecePositions.Length; i++)
        {
            GetNextPiece(_nextPiecePositions[i].position);
        }
    }

    private void Update()
    {
        CheckTimer();
    }

    void CheckTimer()
    {
        if (_timerPaused)
        {
            return;
        }

        if (_gameTime <= 0)
        {
            return;
        }

        _gameTime -= Time.deltaTime;

        if (_gameTime <= 0)
        {
            _gameTime = 0;
            _endGame = true;
            ManagerLocator.Instance.Sound.StopMusic();
            ManagerLocator.Instance.Sound.TimeUp();
            Invoke(nameof(StartEndMenu), 1f);
            //finish game
            return;
        }

        ManagerLocator.Instance.UI.UpdateTimeLeft(_gameTime);

        if (_gameTime <= _runningOutTime && !_isRunningOutOfTime)
        {
            _isRunningOutOfTime = true;
            ManagerLocator.Instance.UI.ChangeTimeColor();
            //sound effect ?
        }
    }

    void StartEndMenu()
    {
        ManagerLocator.Instance.Sound.StartMusicEnd();
        ManagerLocator.Instance.UI.StartEndMenu();
    }

    public void PauseTimer()
    {
        _timerPaused = true;
    }

    public void ResumeTime()
    {
        _timerPaused = false;
    }

    void GenerateBag()
    {
        _nextPieces.Clear();

        for (int i = 0; i < _bagOfPieces.Bag.Length; i++)
        {
            for (int c = 0; c < _bagOfPieces.Bag[i]._amount; c++)
            {
                _nextPieces.Add(Instantiate(_bagOfPieces.Bag[i]._piece, _hiddenPiecesPosition.position, Quaternion.identity));
            }
        }

        _nextPieces.Shuffle();
    }

    public void GetNextPiece(Vector3 position)
    {
        if (1 > _nextPieces.Count)
        {
            GenerateBag();
        }

        _nextPieces[0].SetStartingPosAndScale(position, _nextPieceScale);
        _nextPieces[0].SetNormalScale(_normalPieceScale);
        _nextPieces[0].ReturnToOriginState();
        _nextPieces.RemoveAt(0);
    }

    public void RemoveBonusTile()
    {
        _bonusOnBoard = false;
        _currentBonusTileBoardSpace = null;
        Destroy(_currentBonusTile.gameObject);
    }

    public void TryAddBonusTile()
    {
        if (_bonusOnBoard)
        {
            return;
        }

        float num = Random.Range(0f, 99f );

        if (num >= _bonusTileAppAppearanceChance)
        {
            return;
        }

        if (!ManagerLocator.Instance.Board.IsAnySpaceFree())
        {
            return;
        }

        _bonusOnBoard = true;

        if (_bonusTilePool.Count < 1)
        {
            GenerateBonusTilePool();
        }

        _currentBonusTile = _bonusTilePool[0];
        _bonusTilePool.RemoveAt(0);
        Tile tile = ManagerLocator.Instance.Board.GetFreeSpace(false);
        _currentBonusTile.StartOnPosition(tile.transform.position);

        _currentBonusTileBoardSpace = tile;
    }

    void GenerateBonusTilePool()
    {
        _bonusTilePool.Clear();

        for (int i = 0; i < _amountOfBonusPool; i++)
        {
            _bonusTilePool.Add(Instantiate(_bonusTile, _hiddenPiecesPosition.position, Quaternion.identity));
        }
    }

    [SerializeField]
    GameObject floatingTextPrefab;
    [SerializeField]
    Canvas canvas;

    public void CreateFloatingText(int score, Vector3 gamePiecePosition)
    {
        GameObject textObj = Instantiate(floatingTextPrefab, canvas.transform);
        TextMeshProUGUI textMesh = textObj.GetComponent<TextMeshProUGUI>();
        textMesh.text = "+" + score.ToString();

        Camera mainCamera = Camera.main;  // Make sure this is the camera rendering the objects
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, gamePiecePosition);
        
        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        Vector2 adjustedPosition = screenPosition - canvas.GetComponent<RectTransform>().sizeDelta / 2f;
        Destroy(textObj, 2f); // Destroys the text after 2 seconds

         // Adjust these values as necessary to position the text correctly
        float offsetX = 100f; // Adjust this value as needed
        float offsetY = 130f; // Adjust this value as needed

        rectTransform.anchoredPosition = new Vector2(adjustedPosition.x + offsetX, adjustedPosition.y + offsetY);

    }
    
    public void CalculatePoints(Piece piece)
    {
        int bonus = 1;
        int points = piece.Points; // Assume piece.Points is the base points value for the piece

        if (piece.HasConnectedTile(_currentBonusTileBoardSpace))
        {
            bonus = 4;
            RemoveBonusTile();
            ManagerLocator.Instance.Sound.BonusTile();
        }
        else
        {
            ManagerLocator.Instance.Sound.DropPieceToBoard();
        }

        int totalPoints = bonus * points;
        ManagerLocator.Instance.Stats.AddPackagePoints(totalPoints);
        ManagerLocator.Instance.Stats.AddMultiplier(piece.GetTilesCount());
        ManagerLocator.Instance.Stats.UpdateDeliveryPoints();

        // Now, we create and display the floating text with the total points
        Vector3 piecePosition = piece.transform.position; // Assuming piece has a reference to its transform
        CreateFloatingText(totalPoints, piecePosition);
    }

    public void Swallow()
    {
        if (_timerPaused || _endGame)
        {
            return;
        }

        _blockedTiles++;
        _timerPaused = true;
        _mouth1.SetActive(false);
        _mouth2.SetActive(true);
        ManagerLocator.Instance.Sound.Swallow();
        ManagerLocator.Instance.Stats.AddPackageToTotalPoints();
        ManagerLocator.Instance.Stats.RestartPoints();
        ManagerLocator.Instance.Board.StartSwallow();
        ManagerLocator.Instance.Board.gameObject.SetActive(false);

        Invoke(nameof(EndSwallow), 1.5f);
    }

    public void EndSwallow()
    {
        ManagerLocator.Instance.Board.gameObject.SetActive(true);
        _mouth1.SetActive(true);
        _mouth2.SetActive(false);
        _timerPaused = false;
    }

    public bool IsPaused()
    {
        return _timerPaused;
    }

    public bool GameEnded()
    {
        return _endGame;
    }
}
