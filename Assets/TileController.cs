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

    public Cell _selectedStartTile;
    public Cell _selectedFinishTile;


    public List<Cell> walkableCells = new List<Cell>();
    public List<Cell> nonWalkableCells = new List<Cell>();

    public List<Cell> testList = new List<Cell>();

    private Cell cell1 = new Cell(1, 1);
    private Cell cell2 = new Cell(3, 5);
    private Cell cell3 = new Cell(2, 2);
    private Cell cell4 = new Cell(3, 5);
    private Cell cell5 = new Cell(11, 11);


    private void Awake()
    {
        _tileMap.size = new Vector3Int(100, 100, 0);
        generateBoard();
        setInitialObstacles();
        testList.Add(cell1);
        testList.Add(cell2);
        testList.Add(cell3);

    }

    private List<Cell> getNeighbours(Cell cell)
    {
        List<Cell> neighbourCells = new List<Cell>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int currentX = cell._xPos + x;
                int currentY = cell._yPos + y;
                //  Debug.Log("X" + currentX + " " + "Y" + currentY);
                if (currentX >= 0 && currentY >= 0 && currentX < BOARD_WIDTH && currentY < BOARD_HEIGHT)
                {

                    neighbourCells.Add(BoardCell[currentX, currentY]);
                    // updateTileVisual(new Vector3Int(BoardCell[currentX, currentY]._xPos, BoardCell[currentX, currentY]._yPos, 0), ContentOfCell.Finish);
                }

            }
        }
        return neighbourCells;
    }
    private bool doesListContainSomething(List<Cell> list, Cell cell)
    {
        //  Debug.Log(list[0]._xPos + "   x    " + cell._xPos;
        //  Debug.Log(list[0]._yPos + "   y    " + cell._yPos);
        bool doesContain = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i]._xPos == cell._xPos && list[i]._yPos == list[i]._yPos)
            {

                // Debug.Log(list[i]._xPos + " " + cell._xPos);
                //   Debug.Log(list[i]._yPos + " " + cell._yPos);
                doesContain = true;
            }
        }
        return doesContain;

    }
    private int getDistance(Cell startCell, Cell endCell)
    {
        int distanceX = Mathf.Abs(startCell._xPos - endCell._xPos);
        int distanceY = Mathf.Abs(startCell._yPos - endCell._yPos);
        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        else
            return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    private void findPath(Cell startCell, Cell targetCell)
    {

        walkableCells.Add(startCell);
        //     startCell.ContentOfCell = ContentOfCell.Walkable;
        //  updateTileVisual(new Vector3Int(startCell._xPos, startCell._yPos, 0), ContentOfCell.Walkable);

        while (walkableCells.Count > 0)
        {
            Cell currentCell = walkableCells[0];
            for (int i = 1; i < walkableCells.Count; i++)
            {
                if (walkableCells[i].GetFCost() < currentCell.GetFCost() || walkableCells[i].GetFCost() == currentCell.GetFCost())
                {
                    if (walkableCells[i]._hCost < currentCell._hCost)
                    {
                        currentCell = walkableCells[i];
                        //  walkableCells[i].ContentOfCell = ContentOfCell.Walkable;
                        //  updateTileVisual(new Vector3Int(walkableCells[i]._xPos, walkableCells[i]._yPos, 0), ContentOfCell.Walkable);
                    }

                }

            }

            walkableCells.Remove(currentCell);
            nonWalkableCells.Add(currentCell);
            // currentCell.ContentOfCell = ContentOfCell.NonWalkable;
            //   updateTileVisual(new Vector3Int(currentCell._xPos, currentCell._yPos, 0), ContentOfCell.NonWalkable);


            if (currentCell == targetCell)
            {
                reversePath(startCell, targetCell);
                return;
            }

            foreach (Cell neighbour in getNeighbours(currentCell))
            {
                if (!neighbour.IsWalkable || nonWalkableCells.Contains(neighbour))
                {
                    continue;
                }

                int newGCostNeighbour = currentCell._gCost + getDistance(currentCell, neighbour);
                if (newGCostNeighbour < neighbour._gCost || !walkableCells.Contains(neighbour))
                {
                    neighbour._gCost = newGCostNeighbour;
                    neighbour._hCost = getDistance(neighbour, targetCell);
                    neighbour.ParentCell = currentCell;
                    if (!walkableCells.Contains(neighbour))
                    {
                        walkableCells.Add(neighbour);
                        // neighbour.ContentOfCell = ContentOfCell.Walkable;
                        //   updateTileVisual(new Vector3Int(neighbour._xPos, neighbour._yPos, 0), ContentOfCell.Walkable);
                    }
                }

            }

        }
    }




    private void reversePath(Cell startCell, Cell targetCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = targetCell;
        while (currentCell != startCell)
        {
            path.Add(currentCell);
            updateTileVisual(new Vector3Int(currentCell._xPos, currentCell._yPos, 0), ContentOfCell.Walkable);
            currentCell = currentCell.ParentCell;
        }
        updateTileVisual(new Vector3Int(targetCell._xPos, targetCell._yPos, 0), ContentOfCell.Finish);
        path.Reverse();
    }
    private void setInitialObstacles()
    {
        for (int x = 0; x < 1000; x++)
        {
            Vector3Int randomNumber = new Vector3Int(Random.Range(0, 100), Random.Range(0, 100), 0);
            //  BoardCell[randomNumber.x, randomNumber.y].ContentOfCell = ContentOfCell.NonWalkable;
            updateTileVisual(randomNumber, ContentOfCell.NonWalkable);
            BoardCell[randomNumber.x, randomNumber.y].IsWalkable = false;
        }
    }

    private void updateTileVisual(Vector3Int tilePos, ContentOfCell cellType)
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
        // Debug.Log("NonWalk " + nonWalkableCells.Count);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int selectedTileCoord = getTileByClick();
            Cell selectedCell = BoardCell[selectedTileCoord.x, selectedTileCoord.y];
            if (selectedCell.IsWalkable)
            {
                if (_selectedStartTile == null)
                {
                    // selectedCell.ContentOfCell = ContentOfCell.Start;
                    _selectedStartTile = selectedCell;
                    updateTileVisual(selectedTileCoord, ContentOfCell.Start);
                }
                else if (_selectedFinishTile == null)
                {
                    //    selectedCell.ContentOfCell = ContentOfCell.Finish;
                    _selectedFinishTile = selectedCell;
                    updateTileVisual(selectedTileCoord, ContentOfCell.Finish);
                }
            }
        }

        if (_selectedStartTile != null && _selectedFinishTile != null)
        {
            findPath(_selectedStartTile, _selectedFinishTile);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("START TILE " + _selectedStartTile._xPos + " " + _selectedStartTile._yPos);
            getNeighbours(_selectedStartTile);

        }


    }
}
