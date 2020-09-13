using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFindingAlgorithm : MonoBehaviour
{
    private const int BOARD_WIDTH = 100;
    private const int BOARD_HEIGHT = 100;

    private Cell[,] BoardCell = new Cell[BOARD_WIDTH, BOARD_HEIGHT];
    private VisualGrid _visualGrid => gameObject.GetComponent<VisualGrid>(); // Reference to visual grid, used to visually represent logic and calculations in this script

    private Cell _selectedStartTile;
    private Cell _selectedFinishTile;


    private List<Cell> walkableCells = new List<Cell>();
    private List<Cell> nonWalkableCells = new List<Cell>();

    private void Awake()
    {
        generateBoard();
        setInitialObstacles();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int selectedTileCoord = _visualGrid.GetTileByClick();
            Cell selectedCell = BoardCell[selectedTileCoord.x, selectedTileCoord.y];
            if (selectedCell.IsWalkable)
            {
                if (_selectedStartTile == null)
                {
                    _selectedStartTile = selectedCell;
                    _visualGrid.UpdateTileVisual(selectedTileCoord, ContentOfCell.Start);
                }
                else if (_selectedFinishTile == null)
                {
                    _selectedFinishTile = selectedCell;
                    _visualGrid.UpdateTileVisual(selectedTileCoord, ContentOfCell.Finish);
                }
            }
        }

        if (_selectedStartTile != null && _selectedFinishTile != null)
        {
            findPath(_selectedStartTile, _selectedFinishTile);
        }
    }

    /// <summary>
    /// Set initial obstacles, generated randomly across the map.
    /// </summary>
    private void setInitialObstacles()
    {
        for (int x = 0; x < 1000; x++)
        {
            Vector3Int randomNumber = new Vector3Int(Random.Range(0, 100), Random.Range(0, 100), 0);
            _visualGrid.UpdateTileVisual(randomNumber, ContentOfCell.NonWalkable);
            BoardCell[randomNumber.x, randomNumber.y].IsWalkable = false;
        }
    }


    /// <summary>
    /// Initialize board of Cells.
    /// </summary>
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
    /// <summary>
    /// Get neighbours of tile, by one from each side on each axis.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
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
                int currentX = cell.XPos + x;
                int currentY = cell.YPos + y;
                if (currentX >= 0 && currentY >= 0 && currentX < BOARD_WIDTH && currentY < BOARD_HEIGHT)
                {
                    neighbourCells.Add(BoardCell[currentX, currentY]);
                }
            }
        }
        return neighbourCells;
    }

    /// <summary>
    /// Checks if certain list contains certain cell.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    private bool doesListContainSomething(List<Cell> list, Cell cell)//Needed to do this workaround because list.Contains() returned wrong value 
    {
        bool doesContain = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].XPos == cell.XPos && list[i].YPos == list[i].YPos)
            {
                doesContain = true;
            }
        }
        return doesContain;

    }

    /// <summary>
    /// Get distance between 2 cells on grid.
    /// </summary>
    /// <param name="startCell"></param>
    /// <param name="endCell"></param>
    /// <returns></returns>
    private int getDistance(Cell startCell, Cell endCell)
    {
        int distanceX = Mathf.Abs(startCell.XPos - endCell.XPos);
        int distanceY = Mathf.Abs(startCell.YPos - endCell.YPos);
        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY);
        else
            return 14 * distanceX + 10 * (distanceY - distanceX);
    }


    /// <summary>
    /// Find closest path from start to end point.
    /// </summary>
    /// <param name="startCell"></param>
    /// <param name="targetCell"></param>
    private void findPath(Cell startCell, Cell targetCell)
    {
        walkableCells.Add(startCell);
        while (walkableCells.Count > 0)
        {
            Cell currentCell = walkableCells[0];
            for (int i = 1; i < walkableCells.Count; i++)
            {
                if (walkableCells[i].GetFCost() < currentCell.GetFCost() || walkableCells[i].GetFCost() == currentCell.GetFCost())
                {
                    if (walkableCells[i].HCost < currentCell.HCost)
                    {
                        currentCell = walkableCells[i];
                    }
                }
            }

            walkableCells.Remove(currentCell);
            nonWalkableCells.Add(currentCell);

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
                int newGCostNeighbour = currentCell.GCost + getDistance(currentCell, neighbour);
                if (newGCostNeighbour < neighbour.GCost || !walkableCells.Contains(neighbour))
                {
                    neighbour.GCost = newGCostNeighbour;
                    neighbour.HCost = getDistance(neighbour, targetCell);
                    neighbour.ParentCell = currentCell;
                    if (!walkableCells.Contains(neighbour))
                    {
                        walkableCells.Add(neighbour);
                    }
                }
            }
        }
    }



    /// <summary>
    /// Trace closest path from end to start point and reverse it.
    /// </summary>
    /// <param name="startCell"></param>
    /// <param name="targetCell"></param>
    private void reversePath(Cell startCell, Cell targetCell)
    {
        List<Cell> path = new List<Cell>();
        Cell currentCell = targetCell;
        while (currentCell != startCell)
        {
            path.Add(currentCell);
            _visualGrid.UpdateTileVisual(new Vector3Int(currentCell.XPos, currentCell.YPos, 0), ContentOfCell.Walkable);
            currentCell = currentCell.ParentCell;
        }
        _visualGrid.UpdateTileVisual(new Vector3Int(targetCell.XPos, targetCell.YPos, 0), ContentOfCell.Finish);
        path.Reverse();
    }
}
