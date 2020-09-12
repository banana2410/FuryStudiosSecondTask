using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ContentOfCell
{
    Walkable,
    NonWalkable,
    Start,
    Finish,
}
public class Cell
{
    public Cell(int xPos, int yPos)
    {
        XPos = xPos;
        YPos = yPos;
    }

    public int XPos;
    public int YPos;

    public int HCost;
    public int GCost;

    public ContentOfCell ContentOfCell;
    public bool IsWalkable = true;

    public Cell ParentCell;


    /// <summary>
    /// Returns FCost (HCost + GCost)
    /// </summary>
    /// <returns></returns>
    public int GetFCost()
    {
        return HCost + GCost;
    }

}
