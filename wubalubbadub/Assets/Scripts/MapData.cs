using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public CellData[,] _grid;
    public string name;
    public string description;

    public MapData (creategrid grid)
    {
        CellData[,] map = new CellData[grid.gridSize, grid.gridSize];
        for (int i = 0; i<grid.gridSize; i++)
        {
            for (int j = 0; j < grid.gridSize; j++)
            {
                CellData cell = new CellData();
                cell.cost = grid.GridOfCells[i,j].Cost;
                //cell.position = grid.GridOfCells[i, i].GridPosition;
                cell.posX = i;
                cell.posY = j;
                cell.poiType = grid.GridOfCells[i,j].CellPOIType;
                cell.cellType = grid.GridOfCells[i,j].CellTileType;
                map[i,j] = cell;
            }
        }
        _grid = map;
    }
}

[System.Serializable]
public class CellData
{
    public int cost;
    //public Vector2 position;
    public int posX;
    public int posY;
    public POIType poiType;
    public CellType cellType;
    public string cellEvents;
}