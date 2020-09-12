using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VisualGrid : MonoBehaviour
{
    public TileBase StartTile, FinishTile, ObstacleTile, EmptyTile;
    private Tilemap _tileMap => gameObject.GetComponent<Tilemap>();

    void Start()
    {
        _tileMap.size = new Vector3Int(100, 100, 0);
    }

    public Vector3Int GetTileByClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _tileMap.WorldToCell(mouseWorldPos);
    }
    public void UpdateTileVisual(Vector3Int tilePos, ContentOfCell cellType)
    {
        switch (cellType)
        {
            case ContentOfCell.Walkable:
                _tileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), EmptyTile);
                break;
            case ContentOfCell.NonWalkable:
                _tileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), ObstacleTile);
                break;
            case ContentOfCell.Start:
                _tileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), StartTile);
                break;
            case ContentOfCell.Finish:
                _tileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), FinishTile);
                break;
            default:
                break;
        }

    }
}
