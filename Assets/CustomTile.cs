using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileContent
{
    Empty,
    Obstacle,
    Start,
    Finish
}
[CreateAssetMenu]
public class CustomTile : Tile
{
    public TileContent TileContent;
    public int GCost;
    public int HCost;
    public int FCost; // G + H
}
