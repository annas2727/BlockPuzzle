using UnityEngine.Tilemaps;
using UnityEngine;

public enum PuzzleShapes
{
    Line5, 
    Line4, 
    Line3, 
    Line2, 
    Line1, 
    T, 
    Square2,
    Square3,
    L1,
    L2,
    Z1,
    Z2
}
[System.Serializable]
public struct PuzzleShapeData
{
    public PuzzleShapes shape;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public void Initialize()
    {
        cells = Data.Cells[shape];
    }
}