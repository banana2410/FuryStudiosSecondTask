using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    private const int BOARD_WIDTH = 100;
    private const int BOARD_HEIGHT = 100;
    public Cell[,] BoardCell = new Cell[BOARD_WIDTH, BOARD_HEIGHT];
    private Tilemap _tileMap => gameObject.GetComponent<Tilemap>();
    public TileBase StartTile, FinishTile, ObstacleTile, CurrentTile, EmptyTile;

    private Cell _selectedStartTile = null;
    private Cell _selectedFinishTile = null;
    private Cell _clickedTile = null;

    private List<Cell> WalkableCells = new List<Cell>();
    private List<Cell> NonWalkableCells = new List<Cell>();

    private void Start()
    {

    }
    private void Awake()
    {
        _tileMap.size = new Vector3Int(100, 100, 0);
        generateBoard();
        setInitialObstacles();
        addCellsToLists();
    }
    private void addCellsToLists()
    {
        for (int x = 0; x < BoardCell.GetLength(0); x++)
        {
            for (int y = 0; y < BoardCell.GetLength(1); y++)
            {
                if (BoardCell[x, y].ContentOfCell == ContentOfCell.Nothing)
                    WalkableCells.Add(BoardCell[x, y]);
                else
                    NonWalkableCells.Add(BoardCell[x, y]);
            }
        }
    }
    private void setInitialObstacles()
    {
        for (int x = 0; x < 1000; x++)
        {
            Vector3Int randomNumber = new Vector3Int(Random.Range(0, 100), Random.Range(0, 100), 0);
            BoardCell[randomNumber.x, randomNumber.y].ContentOfCell = ContentOfCell.Obstacle;
            updateTileVisual(randomNumber);
        }
    }

    private CustomTile getTile(Vector3Int position)
    {
        return _tileMap.GetTile<CustomTile>(position);
    }
    private void updateTileVisual(Vector3Int i)
    {
        ContentOfCell contentOfCell = BoardCell[i.x, i.y].ContentOfCell;
        switch (contentOfCell)
        {
            case ContentOfCell.Nothing:
                _tileMap.SetTile(new Vector3Int(i.x, i.y, 0), EmptyTile);
                break;
            case ContentOfCell.Obstacle:
                _tileMap.SetTile(new Vector3Int(i.x, i.y, 0), ObstacleTile);
                break;
            case ContentOfCell.Start:
                _tileMap.SetTile(new Vector3Int(i.x, i.y, 0), StartTile);
                break;
            case ContentOfCell.Finish:
                _tileMap.SetTile(new Vector3Int(i.x, i.y, 0), FinishTile);
                break;
            default:
                break;
        }

    }
    private void generateBoard()
    {
        for (int x = 0; x < BoardCell.GetLength(0); x++)
        {
            for (int y = 0; y < BoardCell.GetLength(1); y++)
            {
                Cell cell = new Cell();
                BoardCell[x, y] = cell;
            }
        }
    }
    private Vector3Int getTileByClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return _tileMap.WorldToCell(mouseWorldPos);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int selectedTileCoord = getTileByClick();
            Cell selectedCell = BoardCell[selectedTileCoord.x, selectedTileCoord.y];
            if (selectedCell.ContentOfCell == ContentOfCell.Nothing)
            {
                if (_selectedStartTile == null)
                {
                    selectedCell.ContentOfCell = ContentOfCell.Start;
                    _selectedStartTile = selectedCell;
                    updateTileVisual(selectedTileCoord);
                }
                else if (_selectedFinishTile == null)
                {
                    selectedCell.ContentOfCell = ContentOfCell.Finish;
                    _selectedFinishTile = selectedCell;
                    updateTileVisual(selectedTileCoord);
                }
            }
        }
    }
}
