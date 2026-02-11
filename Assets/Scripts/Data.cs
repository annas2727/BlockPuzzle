using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

    // Shape cell definitions relative to pivot (0,0)
    public static readonly Dictionary<PuzzleShapes, Vector2Int[]> Cells =
        new Dictionary<PuzzleShapes, Vector2Int[]>()
    {
        // ===== Lines =====
        { PuzzleShapes.Line5, new Vector2Int[] {
            new Vector2Int(-2, 0),
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 0),
            new Vector2Int( 1, 0),
            new Vector2Int( 2, 0)
        }},

        { PuzzleShapes.Line4, new Vector2Int[] {
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 0),
            new Vector2Int( 1, 0),
            new Vector2Int( 2, 0)
        }},

        { PuzzleShapes.Line3, new Vector2Int[] {
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 0),
            new Vector2Int( 1, 0)
        }},

        { PuzzleShapes.Line2, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0)
        }},

        { PuzzleShapes.Line1, new Vector2Int[] {
            new Vector2Int(0, 0)
        }},

        // ===== T Shape =====
        { PuzzleShapes.T, new Vector2Int[] {
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 0),
            new Vector2Int( 1, 0),
            new Vector2Int( 0, 1)
        }},

        // ===== Squares =====
        { PuzzleShapes.Square2, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, 1)
        }},

        { PuzzleShapes.Square3, new Vector2Int[] {
            new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0),
            new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1),
            new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2)
        }},

        // ===== L Shapes =====
        { PuzzleShapes.L1, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, 2),
            new Vector2Int(1, 0)
        }},

        { PuzzleShapes.L2, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(2, 1), 
            new Vector2Int(2, 2)
        }},

        // ===== Z Shapes =====
        { PuzzleShapes.Z1, new Vector2Int[] {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(2, 0)
        }},

        { PuzzleShapes.Z2, new Vector2Int[] {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(2, 1)
        }},
    };
}
