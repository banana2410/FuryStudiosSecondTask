using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ContentOfCell
{
    Nothing,
    Obstacle,
    Start,
    Finish
}
public enum TypeOfValue
{
    H,
    G,
    F
}

public class Cell
{
    public Cell(int xPos, int yPos)
    {
        _xPos = xPos;
        _yPos = yPos;
    }
    public ContentOfCell ContentOfCell;
    public int _xPos;
    public int _yPos;
    private int _hCost;
    private int _gCost;

    public void SetValue(TypeOfValue typeOfValue, int value)
    {
        switch (typeOfValue)
        {
            case TypeOfValue.H:
                _hCost = value;
                break;
            case TypeOfValue.G:
                _gCost = value;
                break;
            default:
                Debug.LogError("Errrrror");
                break;
        }
    }
    public int GetFCost()
    {
        return _hCost + _gCost;
    }

}
