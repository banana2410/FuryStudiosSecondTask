using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellGrid : MonoBehaviour
{
    public List<Vector2> ObstacleSpots = new List<Vector2>();

    public GameObject CellPrefab;


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
       
    }
    private void generateObstacles()
    {
        for (int x = 0; x < ObstacleSpots.Count; x++)
        {
            //Board[(int)ObstacleSpots[x].x, (int)ObstacleSpots[x].y].SetContentOfCell(ContentOfCell.Obstacle);
        }
    }



    private void setInitialPositions()
    {

       // Board[0, 3].SetContentOfCell(ContentOfCell.Obstacle);

    }
}


