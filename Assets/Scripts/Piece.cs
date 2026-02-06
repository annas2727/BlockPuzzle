using UnityEngine;
using UnityEngine.InputSystem;
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

        foreach (Transform child in transform) { //clear old blocks
            Destroy(child.gameObject);
        }

        this.cells = new Vector3Int[data.cells.Length];
        
        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];

            GameObject blockObject = new GameObject("PieceBlock");
            blockObject.transform.SetParent(this.transform);
            
            // Position the block relative to the parent Piece
            blockObject.transform.localPosition = new Vector3(data.cells[i].x, data.cells[i].y, 0);

            // Add the SpriteRenderer and assign the sprite from the Tile
            SpriteRenderer sr = blockObject.AddComponent<SpriteRenderer>();
            if (data.tile != null) {
                sr.sprite = data.tile.sprite; // Uses the sprite assigned to your Tile asset
            }
            
            sr.sortingOrder = 10; //appears in front
        }

        // move piece to spawn point
        transform.position = board.tilemap.CellToWorld(position) + board.tilemap.tileAnchor;
        gameObject.SetActive(true);
    }

   private void Update()
    {
        // Check if the screen was touched or mouse was clicked
        bool inputStarted = Pointer.current.press.wasPressedThisFrame;
        bool inputHeld = Pointer.current.press.isPressed;
        bool inputEnded = Pointer.current.press.wasReleasedThisFrame;

        if (inputStarted) {
            HandleInputStart();
        }
        else if (inputHeld && isDragging) {
            HandleInputMove();
        }
        else if (inputEnded && isDragging) {
            HandleInputEnd();
        }
    }

    private void HandleInputStart()
    {
        // Shoot a ray to see if we clicked THIS piece
        Vector2 mousePos = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject) {
            isDragging = true;
            dragOffset = transform.position - (Vector3)mousePos;
        }
    }

    private void HandleInputMove()
    {
        transform.position = (Vector3)GetMouseWorldPosition() + dragOffset;
    }

    private void HandleInputEnd()
    {
        isDragging = false;
        Vector3Int droppedGridPos = board.tilemap.WorldToCell(transform.position);
        
        if (board.IsValidPlacement(this, droppedGridPos)) {
            this.position = droppedGridPos;
            board.Set(this);
        } else {
            transform.position = board.tilemap.CellToWorld(board.spawnPosition) + board.tilemap.tileAnchor;
        }
    }

        private Vector3 GetMouseWorldPosition()
    {
        // New Input System way to get position
        Vector2 mousePos = Pointer.current.position.ReadValue();
        Vector3 mouseScreenPosition = new Vector3(mousePos.x, mousePos.y, 0);
        
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

}