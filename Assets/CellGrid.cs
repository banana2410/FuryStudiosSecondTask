using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellGrid : MonoBehaviour
{
    public List<Vector2> ObstacleSpots = new List<Vector2>();
    public Cell[,] Board = new Cell[BOARD_WIDTH, BOARD_HEIGHT];
    public GameObject CellPrefab;
    private const int BOARD_WIDTH = 10;
    private const int BOARD_HEIGHT = 10;


    private List<Cell> _openCells = new List<Cell>();
    private List<Cell> _closedCells = new List<Cell>();

    
    
    private void Start()
    {
      /*  Board[0, 0].SetContentOfCell(ContentOfCell.Start);
        Board[9, 9].SetContentOfCell(ContentOfCell.Finish);
        generateObstacles();*/

    }
    private void Awake()
    {
        generateBoard();
    }
    private void generateObstacles()
    {
        for (int x = 0; x < ObstacleSpots.Count; x++)
        {
            Board[(int)ObstacleSpots[x].x, (int)ObstacleSpots[x].y].SetContentOfCell(ContentOfCell.Obstacle);
        }
    }


    private void generateBoard()
    {
        for (int x = 0; x < Board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.GetLength(1); y++)
            {
                Cell cell = Instantiate(CellPrefab, position: new Vector3(x, y), Quaternion.identity).GetComponent<Cell>();
                cell.SetValue(TypeOfValue.H, x);
                cell.SetValue(TypeOfValue.G, y);
                Board[x, y] = cell;
            }
        }


    }
    private void setInitialPositions()
    {

        Board[0, 3].SetContentOfCell(ContentOfCell.Obstacle);

    }
}


