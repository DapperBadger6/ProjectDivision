using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CellClass
{
    [SerializeField] string _name;
    [SerializeField] GameObject _cellGO;
    [SerializeField] GameObject _cellPOIGO;
    private Vector2 _position;
    int _cost = 1;
    int _distance = 0;
    CellClass _parent;
    [SerializeField] public HashSet<CellClass> adjacentCells = new HashSet<CellClass>();
    bool _occupied = false;
    private CellType _cellType = CellType.Plains;
    private POIType _cellPOI = POIType.None;

    public string Name
    {
        get { return _name;}
        set { _name = value; }
    }
    public bool Occupied
    {
        get{ return _occupied; }
        set{ _occupied = value; }
    }
    public Vector2 GridPosition
    {
        get { return _position; }
        set { _position = value; }
    }
    public GameObject CellGO
    {
        get { return _cellGO; }
        set { _cellGO = value; }
    }
    public GameObject CellPOIGO
    {
        get { return _cellPOIGO; }
        set { _cellPOIGO = value; }
    }
    public int Cost
    {
        get { return _cost; }
        set { _cost = value; }
    }
    public int Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }
    public CellClass ParentCell
    {
        get { return _parent; }
        set { _parent = value; }
    }
    public CellType CellTileType
    {
        get { return _cellType; }
        set { _cellType = value; }
    }
    public POIType CellPOIType
    {
        get { return _cellPOI; }
        set { _cellPOI = value; }
    }
    public HashSet<CellClass> AdjacentCells
    {
        get { return adjacentCells; }
        set { adjacentCells = value; }
    }
    public float XPosition
    {
        get { return _position.x; }
    }
    public float YPosition
    {
        get { return _position.y; }
    }
}