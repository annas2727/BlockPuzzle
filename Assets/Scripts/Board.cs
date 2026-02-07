using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public static int score = 0;
    public static int combo = 0;
    public static int attempts = 3; 
    Text scoreText;
    Text comboText;
    GameObject light1;
    GameObject light2;
    GameObject light3;
    public Tilemap tilemap { get; set; }
    bool[,] IsOccupied = new bool[8, 8];

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
        light1 = GameObject.Find("Light1On");
        light2 = GameObject.Find("Light2On");
        light3 = GameObject.Find("Light3On");
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        comboText = GameObject.Find("ComboText").GetComponent<Text>();
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
                
        activeInstance.Initialize(this, spawnPosition, data);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++) //bake tile into grid
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
            Debug.Log ("tile position: " + tilePosition);
            IsOccupied[tilePosition.x + boardSize.x/2, tilePosition.y + boardSize.y/2] = true;
        }


        score += piece.cells.Length;
        UpdateScore();
        ProcessLines();
        SpawnPiece();
    }

    public bool IsValidPlacement(Piece piece, Vector3Int position)
    {
        foreach (Vector3Int cell in piece.cells) 
        {
            Vector3Int tilePosition = cell + position + boardSize/2;
            
            //checks if it in bounds of board
            bool inX = tilePosition.x >= boardOrigin.x && tilePosition.x < boardOrigin.x + boardSize.x;
            bool inY = tilePosition.y >= boardOrigin.y && tilePosition.y < boardOrigin.y + boardSize.y;

            if (!inX || !inY) {
                Debug.Log($"FAILED BOUNDS: Tile {tilePosition} is outside X({boardOrigin.x} to {boardOrigin.x + boardSize.x}) or Y({boardOrigin.y} to {boardOrigin.y + boardSize.y})");
                return false;
            }
            
            if (IsOccupied[tilePosition.x, tilePosition.y]) {
                Debug.Log("failed placement - overlap");
                return false;
            }
            Debug.Log("Placement is VALID");

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
        for (int y = boardOrigin.y - boardSize.y/2; y < boardOrigin.y + boardSize.y/2; y++) {
            if (IsRowFull(y)) fullRows.Add(y);
        }

        //get columns
        for (int x = boardOrigin.x - boardSize.x/2; x < boardOrigin.x + boardSize.x/2; x++) {
            if (IsColumnFull(x)) fullCols.Add(x);
        }
        
        //clear rows
        foreach (int y in fullRows) {
            for (int x = boardOrigin.x - boardSize.x/2; x < boardOrigin.x + boardSize.x/2; x++) {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
                IsOccupied[x + boardSize.x/2, y + boardSize.y/2] = false;
            }
        }

        //clear columns
        foreach (int x in fullCols) {
            for (int y = boardOrigin.y - boardSize.y/2; y < boardOrigin.y + boardSize.y/2; y++) {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
                IsOccupied[x + boardSize.x/2, y + boardSize.y/2] = false;
            }
        }

        AddComboScore(fullRows.Count + fullCols.Count);
    }
    public void AddComboScore(int linesCleared)
    {
        //verify combo
        if (linesCleared > 0 && combo > 0)
        {
            // RESET: Player cleared a line, give them all attempts back
            attempts = 3;
            combo++;
            light1.SetActive(true);
            light2.SetActive(true);
            light3.SetActive(true);
        }
        else 
        {
            // FAIL: Player placed a piece but cleared nothing
            attempts--;
            if (attempts == 2) {
                light1.SetActive(false);
            } 
            else if (attempts == 1) {
                light2.SetActive(false);
            } 
            else if (attempts == 0) {
                // GAME OVER / RESET COMBO
                light3.SetActive(false);
                combo = 0;    
                attempts = 3;
                light1.SetActive(true);
                light2.SetActive(true);
                light3.SetActive(true);            
            }
        }

        score += combo * linesCleared * boardSize.x; 
        UpdateScore();
        UpdateCombo();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateCombo()
    {
        comboText.text = "Combo: \nx" + combo;
    }

    public void ClearBoard()
    {
        for (int x = boardOrigin.x - boardSize.x/2; x < boardOrigin.x + boardSize.x/2; x++) {
            for (int y = boardOrigin.y - boardSize.y/2; y < boardOrigin.y + boardSize.y/2; y++) {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
                IsOccupied[x + boardSize.x/2, y + boardSize.y/2] = false;
            }
        }
    }

    public void Restart()
    {
        score = 0; 
        attempts = 3; 
        UpdateScore(); 
        ClearBoard();
        light1.SetActive(true);
        light2.SetActive(true);
        light3.SetActive(true);
    }
}