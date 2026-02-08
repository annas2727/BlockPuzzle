using UnityEngine;
using UnityEngine.InputSystem;
public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public PuzzleShapeData data { get; private set; }
    public Vector3Int position { get; private set; }   
    public Vector3Int spawnPosition {get; set; }
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

    foreach (Transform child in transform) { 
        Destroy(child.gameObject);
    }

    cells = new Vector3Int[data.cells.Length];
    
    Quaternion rotation = transform.rotation;

    for (int i = 0; i < data.cells.Length; i++)
    {
        Vector3 localPos = new Vector3(data.cells[i].x, data.cells[i].y, 0);

        GameObject blockObject = new GameObject("PieceBlock");
        blockObject.transform.SetParent(this.transform);
        blockObject.transform.localPosition = localPos;

        blockObject.transform.rotation = Quaternion.identity; 

        SpriteRenderer sr = blockObject.AddComponent<SpriteRenderer>();
        if (data.tile != null) {
            sr.sprite = data.tile.sprite;
        }
        sr.sortingOrder = 10;

        Vector3 rotatedPoint = rotation * localPos;
        cells[i] = new Vector3Int(
                Mathf.RoundToInt(rotatedPoint.x),
                Mathf.RoundToInt(rotatedPoint.y),
                0
            );
        }

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
            position = droppedGridPos;
            board.Set(this);
        } else {
            transform.position = board.tilemap.CellToWorld(spawnPosition) + board.tilemap.tileAnchor;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector2 mousePos = Pointer.current.position.ReadValue();
        Vector3 mouseScreenPosition = new Vector3(mousePos.x, mousePos.y, mainCamera.WorldToScreenPoint(transform.position).z);
        
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }

}