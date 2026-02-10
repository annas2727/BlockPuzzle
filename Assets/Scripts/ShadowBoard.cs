using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ShadowBoard : MonoBehaviour
{
    public Tilemap shadowMap { get; set; }
    public Sprite shadowSprite; 

    private void Awake()
    {
        shadowMap = GetComponentInChildren<Tilemap>();
    }

    public void Set(Piece piece, Vector3Int gridPos)
    {
        for (int i = 0; i < piece.cells.Length; i++) //bake tile into grid
        {
            Vector3Int tilePosition = piece.cells[i] + gridPos;
            Tile shadowTile = ScriptableObject.CreateInstance<Tile>();
            shadowTile.sprite = shadowSprite;
            shadowTile.color = Color.white;

            shadowMap.SetTile(tilePosition, shadowTile);
        }
    }

    public void Clear(Piece piece, Vector3Int gridPos)
    {
        for (int i = 0; i < piece.cells.Length; i++) 
        {
        Vector3Int tilePosition = piece.cells[i] + gridPos;
        shadowMap.SetTile(tilePosition, null);
        }
    }

}
