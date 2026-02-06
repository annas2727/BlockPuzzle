using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public PuzzleShapeData data { get; private set; }
    public Vector3Int position { get; private set; }   
    public Vector3Int[] cells { get; private set; }
    public Vector3 dragOffset;
    private Camera mainCamera;
    private bool isDragging = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

   public void Initialize(Board board, Vector3Int position, PuzzleShapeData data)
   {
        this.data = data;
        this.board = board;
        this.position = position;

        // 1. Clear any old blocks from a previous spawn
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // 2. Map the data cells to our logic array
        this.cells = new Vector3Int[data.cells.Length];
        
        // 3. Loop through the shape data to draw each sprite
        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];

            // Create a new child GameObject for this specific block
            GameObject blockObject = new GameObject("PieceBlock");
            blockObject.transform.SetParent(this.transform);
            
            // Position the block relative to the parent Piece
            blockObject.transform.localPosition = new Vector3(data.cells[i].x, data.cells[i].y, 0);

            // Add the SpriteRenderer and assign the sprite from the Tile
            SpriteRenderer sr = blockObject.AddComponent<SpriteRenderer>();
            if (data.tile != null) {
                sr.sprite = data.tile.sprite; // Uses the sprite assigned to your Tile asset
            }
            
            // Ensure it shows up in front of the background
            sr.sortingOrder = 10; 
        }

        // 4. Move the whole piece to the spawn point
        transform.position = board.tilemap.CellToWorld(position) + board.tilemap.tileAnchor;
        gameObject.SetActive(true);
    }
    private void OnMouseDown()
    {
        isDragging = true;
        dragOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + dragOffset;
    }

    private void OnMouseUp()
    {
        {
        isDragging = false;

        Vector3Int droppedGridPos = board.tilemap.WorldToCell(transform.position);
        
        if (board.IsValidPlacement(this, droppedGridPos)) {
            this.position = droppedGridPos;
            board.Set(this); // Bake into tilemap
            gameObject.SetActive(false); 
            board.SpawnPiece();
        } else {
            // Snap back to spawn if placement is invalid
            transform.position = board.tilemap.CellToWorld(board.spawnPosition) + board.tilemap.tileAnchor;
        }
    }    }
    
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }




}