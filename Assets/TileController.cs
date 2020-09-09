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

    }
    private List<Cell> getNeighbours(Cell cell)
    {
        List<Cell> neighbourCells = new List<Cell>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int currentX = cell._xPos + x;
                int currentY = cell._yPos + y;
                if (currentX > 0 && currentY > 0 && currentX < BOARD_WIDTH && currentY < BOARD_WIDTH)
                    neighbourCells.Add(cell);
            }
        }
        return neighbourCells;
    }
    private int getDistance(Cell startCell, Cell endCell)
    {
        int distanceX = Mathf.Abs(startCell._xPos - endCell._xPos);
        int distanceY = Mathf.Abs(startCell._yPos - endCell._yPos);
        if (distanceY > distanceX)
            return distanceX + (distanceY - distanceX);
        return distanceY + (distanceX - distanceY);
    }
    private void findPath(Cell startCell, Cell endCekk)
    {
        List<Cell> walkableCells = new List<Cell>();
        List<Cell> nonWalkableCells = new List<Cell>();
        walkableCells.Add(startCell);
        while(walkableCells.Count> 0)
        {
            Cell currentCell = walkableCells[0];
            for (int i = 0; i < walkableCells.Count; i++)
            {
                if(walkableCells[i].GetFCost()<currentCell.GetFCost()||walkableCells[i].GetFCost())
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

    private void updateTileVisual(Vector3Int tilePos)
    {
        ContentOfCell contentOfCell = BoardCell[tilePos.x, tilePos.y].ContentOfCell;
        switch (contentOfCell)
        {
            case ContentOfCell.Nothing:
                _tileMap.SetTile(new Vector3Int(tilePos.x, tilePos.y, 0), EmptyTile);
                break;
            case ContentOfCell.Obstacle:
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
    private void generateBoard()
    {
        for (int x = 0; x < BoardCell.GetLength(0); x++)
        {
            for (int y = 0; y < BoardCell.GetLength(1); y++)
            {
                Cell cell = new Cell(x, y);
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
                    Debug.Log(getNeighbours(selectedCell).Count);
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
