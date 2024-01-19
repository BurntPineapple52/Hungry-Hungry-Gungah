using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PieceAmount
{
    public Piece _piece;
    [Min(1)]
    public int _amount;
}

[CreateAssetMenu(fileName = "Bag of Pieces", menuName = "ScriptableObjects/Bag of Pieces")]
public class BagOfPieces : ScriptableObject
{
    [SerializeField]
    PieceAmount[] _bagOfPieces = new PieceAmount[0];

    public PieceAmount[] Bag { get => _bagOfPieces; }
}
