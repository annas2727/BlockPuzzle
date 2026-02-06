using UnityEngine;

using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;



public class Board : MonoBehaviour
{
    public static int score = 0;
    public static int combo = 0;
    public static int attempts = 3; 
    public Text scoreText;

    public Tilemap tilemap { get; private set; }
    

[Header("References")]
    public Piece piecePrefab; 
    private Piece activeInstance;
    public PuzzleShapeData[] puzzleShapeData;

    [Header("Settings")]
    public Vector3Int spawnPosition;

    [Header("Board Dimensions")]
    public Vector3Int boardSize = new Vector3Int(8, 8, 0);
    public Vector3Int boardOrigin = new Vector3Int(0,0,0);

    private void Start()
    {
        SpawnPiece();
    }

    private void Awake()
{
    tilemap = GetComponentInChildren<Tilemap>();
    

    for (int i = 0; i < puzzleShapeData.Length; i++) {
        puzzleShapeData[i].Initialize();
    }
}

    public void SpawnPiece()
    {
        if (activeInstance == null) {
            activeInstance = Instantiate(piecePrefab);
        }

        activeInstance.gameObject.SetActive(true);
        
        int random = Random.Range(0, this.puzzleShapeData.Length);
        PuzzleShapeData data = this.puzzleShapeData[random];  
                
        this.activeInstance.Initialize(this, spawnPosition, data);
    }


    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++) //bake tile into grid
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }

        score += piece.cells.Length;
        UpdateScore();
        ProcessLines();
        SpawnPiece();
    }

    public bool IsValidPlacement(Piece piece, Vector3Int position)
    {
        foreach (var cell in piece.cells) 
        {
            Vector3Int tilePosition = cell + position + boardSize/2;

            bool inX = tilePosition.x >= boardOrigin.x && tilePosition.x < boardOrigin.x + boardSize.x;
            bool inY = tilePosition.y >= boardOrigin.y && tilePosition.y < boardOrigin.y + boardSize.y;

            if (!inX || !inY) {
                Debug.Log($"FAILED BOUNDS: Tile {tilePosition} is outside X({boardOrigin.x} to {boardOrigin.x + boardSize.x}) or Y({boardOrigin.y} to {boardOrigin.y + boardSize.y})");
                return false;
            }

            if (tilemap.HasTile(tilePosition)) {
                return false; 
            }
        }
        return true;
    }

    public bool IsRowFull(int y)
    {
        for (int x = boardOrigin.x - boardSize.x/2; x < boardOrigin.x + boardSize.x/2; x++)
        {
            if (!tilemap.HasTile(new Vector3Int(x, y, 0))) {
                return false;
            }
        }
        return true;
    }

    public bool IsColumnFull(int x)
    {
        for (int y = boardOrigin.y - boardSize.y/2; y < boardOrigin.y + boardSize.y/2; y++)
        {
            if (!tilemap.HasTile(new Vector3Int(x, y, 0))) {
                return false;
            }
        }
        return true;
    }


    public void ProcessLines()
    {
        List<int> fullRows = new List<int>();
        List<int> fullCols = new List<int>();

        //get rows
        for (int y = boardOrigin.y; y < boardOrigin.y + boardSize.y; y++) {
            if (IsRowFull(y)) fullRows.Add(y);
        }

        //get columns
        for (int x = boardOrigin.x; x < boardOrigin.x + boardSize.x; x++) {
            if (IsColumnFull(x)) fullCols.Add(x);
        }

        //clear rows
        foreach (int y in fullRows) {
            for (int x = boardOrigin.x - boardSize.x/2; x < boardOrigin.x + boardSize.x/2; x++)
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
        }

        //clear columns
        foreach (int x in fullCols) {
            for (int y = boardOrigin.y - boardSize.y/2; y < boardOrigin.y + boardSize.y/2; y++)
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
        }

        AddComboScore(fullRows.Count + fullCols.Count);
    }
    public void AddComboScore(int linesCleared)
    {
        //fix
        Debug.Log("Score: " + score);
        score += combo * linesCleared * 8; 
        combo++;
        UpdateScore();

    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }


}