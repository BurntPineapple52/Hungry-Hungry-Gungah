using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerLocator : MonoBehaviour
{
    static ManagerLocator _instance;
    public static ManagerLocator Instance { get => _instance; }

    [SerializeField]
    GridManager _board;

    [SerializeField]
    GameManager _gameManager;

    [SerializeField]
    StatsManager _statsManager;

    [SerializeField]
    UIManager _uiManager;

    [SerializeField]
    SoundManager _sound;

    public GridManager Board { get => _board; }
    public GameManager Game { get => _gameManager; }
    public StatsManager Stats { get => _statsManager; }
    public UIManager UI { get => _uiManager; }
    public SoundManager Sound { get => _sound; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            return;
        }

        if (_instance != this)
        {
            Destroy(this);
        }
    }
}
