using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    private Tilemap _tileMap => gameObject.GetComponent<Tilemap>();
    public Tile[,] Board = new Tile[100, 100];
    public TileBase StartTile, FinishTile, ObstacleTile, CurrentTile, EmptyTile;

    private TileBase _selectedStartTile = null;
    private TileBase _selectedFinishTile = null;
    private TileBase _clickedTile = null;
    // public List<Vector2Int> ObstacleSpots = new List<Vector2Int>();

    private void Start()
    {
        _tileMap.size = new Vector3Int(100, 100, 0);
        /*  for (int i = 0; i < ObstacleSpots.Count; i++)
          {
              TileMap.SetTile((Vector3Int)ObstacleSpots[i], EmptyTile);
          }*/
        for (int x = 0; x < 1200; x++)
        {
            _tileMap.SetTile((Vector3Int)new Vector2Int(Random.Range(0, 101), Random.Range(0, 101)), ObstacleTile);
        }
    }

    private CustomTile getTile(Vector3Int position)
    {
        return _tileMap.GetTile<CustomTile>(position);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = _tileMap.WorldToCell(mouseWorldPos);
            _clickedTile = getTile(coordinate);
            if (_selectedStartTile == null)
            {
                _tileMap.SetTile(coordinate, StartTile);
                _selectedStartTile = getTile(coordinate);
            }
            else
            {
                _tileMap.SetTile(coordinate, FinishTile);
                _selectedStartTile = getTile(coordinate);
            }


        }
    }
}
