using UnityEngine;

using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class Board : MonoBehaviour
{
    public static int score = 0;
    public static int combo = 0;
    public Text scoreText;

    public Tilemap tilemap { get; private set; }
    

[Header("References")]
    public Piece piecePrefab; 
    private Piece activeInstance;
    public PuzzleShapeData[] puzzleShapeData;

    [Header("Settings")]
    public Vector3Int spawnPosition;
    
    [Header("Board Dimensions")]
    public Vector2Int boardSize = new Vector2Int(8, 8);
    public Vector2Int boardOrigin = new Vector2Int(0, 0);

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
        AddScore();
    }


    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++) //bake tile into grid
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }

        AddScore(); 
        SpawnPiece();
    }

    public bool IsValidPlacement(Piece piece, Vector3Int position)
    {
        foreach (var cell in piece.cells) 
        {
            Vector3Int tilePosition = cell + position;

            if (tilePosition.x < boardOrigin.x || tilePosition.x >= boardOrigin.x + boardSize.x ||
                tilePosition.y < boardOrigin.y || tilePosition.y >= boardOrigin.y + boardSize.y) {
                    Debug.Log ("out of bounds");
                return false; 
            }

            if (tilemap.HasTile(tilePosition)) {
                return false; 
            }
        }
        return true;
    }

    public void AddScore()
    {
        //fix
        score += 10 * combo; 
        combo++;
    }

    public void Update()
    {
        scoreText.text = "Score: " + score;
    }

}