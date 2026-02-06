using UnityEngine;

using UnityEngine.Tilemaps;


public class Board : MonoBehaviour

{

    public Tilemap tilemap { get; private set; }

[Header("References")]
    public Piece piecePrefab; 
    private Piece activeInstance;
    public PuzzleShapeData[] puzzleShapeData;

    [Header("Settings")]
    public Vector3Int spawnPosition;

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
        // Safety check to prevent the crash
        if (activeInstance == null) {
            activeInstance = Instantiate(piecePrefab);
        }

        if (puzzleShapeData.Length == 0) {
            Debug.LogError("Puzzle Shape Data array is empty!");
            return;
        }
        activeInstance.gameObject.SetActive(true);
        
        int random = Random.Range(0, this.puzzleShapeData.Length);
        PuzzleShapeData data = this.puzzleShapeData[random];  
                
        this.activeInstance.Initialize(this, spawnPosition, data);
    }


    public void Set(Piece piece)
    {
        // Bake the tiles into the grid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public bool IsValidPlacement(Piece piece, Vector3Int position)
    {
        foreach (var cell in piece.cells) {
            Vector3Int tilePosition = cell + position;
            if (tilemap.HasTile(tilePosition)) {
                return false; // Already a block there
            }

        }
        return true;
    }

}