using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField]
    int _maxMultiplierAdded = 10;

    int _totalPoints = 0;

    int _currentPackagePoints = 0;
    float _multiplier = 1;
    int _totalPointsDelivery = 0;

    float _currentMultiplierPerTile = 0;

    public int TotalPoints { get => _totalPoints; }

    private void Start()
    {
        RestartPoints();
    }

    public void UpdateCurrentMultiplier(int tilesInTheBoard)
    {
        if (tilesInTheBoard == 0)
        {
            tilesInTheBoard++;
        }

        _currentMultiplierPerTile = (float)_maxMultiplierAdded / (float)tilesInTheBoard;
    }

    public void RestartPoints()
    {
        _currentPackagePoints = 0;
        _multiplier = 1;
        _totalPointsDelivery = 0;

        ManagerLocator.Instance.UI.UpdateScore(0);
        ManagerLocator.Instance.UI.UpdateMultiplier(1);
        ManagerLocator.Instance.UI.UpdateMultipliedScore(0);
    }

    public void AddPackagePoints(int num)
    {
        _currentPackagePoints += num;
        ManagerLocator.Instance.UI.UpdateScore(_currentPackagePoints);
    }

    public void AddMultiplier(int tilesAmount)
    {
        _multiplier += (tilesAmount * _currentMultiplierPerTile);
        ManagerLocator.Instance.UI.UpdateMultiplier(_multiplier);
    }

    public void UpdateDeliveryPoints()
    {
        _totalPointsDelivery = Mathf.RoundToInt(_currentPackagePoints * _multiplier);
        ManagerLocator.Instance.UI.UpdateMultipliedScore(_totalPointsDelivery);
    }

    public void AddPackageToTotalPoints()
    {
        _totalPoints += _totalPointsDelivery;
        ManagerLocator.Instance.UI.UpdateTotalScore(_totalPoints);
    }
}
