using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creategrid : MonoBehaviour {
    #region Fields
    // Use this region for fields
    [SerializeField]
    public int gridSize = 25;
    [SerializeField]
    private float cellSize = 1.1f;
    [SerializeField]
    private int testMoveDistance = 10;
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private GameObject poiPrefab;
    [SerializeField]
    Transform cellGridParent;
    [SerializeField]
    public CellClass[,] GridOfCells;
    [SerializeField]
    List<CellClass> colouredCells;
    bool editMode = true;
    public CellType cellType = CellType.Plains;
    public enum EditorModes { editTile,editPOI,off };
    public EditorModes editorMode = EditorModes.off;
    public POIType poiType = POIType.Bunker;
    public Material[] cellMaterials;
    public Material[] poiMaterials;
    public InputField input;

    #endregion

    void Start()
    {
        colouredCells = new List<CellClass>();
        if (cellPrefab != null && cellGridParent != null)
        {
            InitialiseGridClasses();
        }
        else
        {
            throw new System.Exception("Cell prefab or grid parent not found!");
        }
        input.text = "Map name...";
    }

    // Update is called once per frame
    void Update()
    {
        /*if (editMode == false)
        {
            //ClickToFindPath();
            //ClickFindPathByAlgorithm();
        }
        else
        {
            //if (Input.GetMouseButton(0))
            ClickToEditCellType();
        }*/
        if (editorMode == EditorModes.editTile)
        {
            if (Input.GetMouseButton(0))
                ClickToEditCellType();
        }
        else if (editorMode == EditorModes.editPOI)
        {
            if (Input.GetMouseButton(0))
                ClickToEditPOIType();
        }
    }

    private void ClickToEditCellType()
    {
        //throw new NotImplementedException();
        // Change cell type of cell clicked on by mouse
        CellClass cell = ClickToFindCell();
        if (cell == null) return;
        else
        {
            Debug.Log("I am doing something");
            // Change cell or whatever here. Need cell type database
            switch (cellType)
            {
                case CellType.Plains:
                    cell.CellTileType = CellType.Plains;
                    cell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[0];
                    break;
                case CellType.Forest:
                    cell.CellTileType = CellType.Forest;
                    cell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[1];
                    break;
                case CellType.Desert:
                    cell.CellTileType = CellType.Desert;
                    cell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[2];
                    break;
                case CellType.River:
                    cell.CellTileType = CellType.River;
                    cell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[3];
                    break;
                case CellType.Wasteland:
                    cell.CellTileType = CellType.Wasteland;
                    cell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[4];
                    break;
            }
            Debug.Log("My tile type now is: "+cell.CellTileType);
        }
    }

    private void ClickToEditPOIType()
    {
        //throw new NotImplementedException();
        // Change cell type of cell clicked on by mouse
        CellClass cell = ClickToFindCell();
        if (cell == null) return;
        else
        {
            Debug.Log("I am doing something");
            Renderer[] renderers = cell.CellGO.GetComponentsInChildren<Renderer>();
            // Change cell or whatever here. Need cell type database
            switch (poiType)
            {
                case POIType.None:
                    cell.CellPOIType = POIType.None;
                    Debug.Log(renderers[1].material.name);
                    renderers[1].material = poiMaterials[0];
                    break;
                case POIType.Bunker:
                    cell.CellPOIType = POIType.Bunker;
                    Debug.Log(renderers[1].material.name);
                    renderers[1].material = poiMaterials[1];
                    break;
                case POIType.Lero:
                    cell.CellPOIType = POIType.Lero;
                    Debug.Log(renderers[1].material.name);
                    renderers[1].material = poiMaterials[2];
                    break;
                case POIType.Onivine:
                    cell.CellPOIType = POIType.Onivine;
                    Debug.Log(renderers[1].material.name);
                    renderers[1].material = poiMaterials[2];
                    break;
                case POIType.FallenLeaf:
                    cell.CellPOIType = POIType.FallenLeaf;
                    Debug.Log(renderers[1].material.name);
                    renderers[1].material = poiMaterials[3];
                    break;
            }
            Debug.Log("My POI type now is: " + cell.CellPOIType);
        }
    }

    // Casts a ray against the cells and finds a path
    private CellClass ClickToFindCell()
    {
        CellClass myCell = null;
        try
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("I hit: " + hit.transform.parent.gameObject.name);

                Debug.Log(hit);
                CellClass cell = CellMeshFromTransform(hit.transform.parent.gameObject);
                if (cell != null)
                {
                    myCell = cell;
                }
                else
                {
                    Debug.Log(hit.transform.parent.gameObject.name);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("This is bullshit");
        }
        return myCell;
    }

    // Finds and returns cell from given gameobject
    private CellClass CellMeshFromTransform(GameObject hitObject)
    {
        foreach (CellClass cell in GridOfCells)
        {
            if (cell.CellGO == hitObject)
            {
                return cell;
            }
        }
        return null;
    }

    // Initialises and instantiates the grid of cells
    private void InitialiseGridClasses()
    {
        GridOfCells = new CellClass[gridSize, gridSize];

        // Create cells
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, y * cellSize);
                GameObject cellMesh = GameObject.Instantiate(cellPrefab, cellPosition, Quaternion.identity, cellGridParent);
                GameObject poiMesh = GameObject.Instantiate(poiPrefab, cellPosition, Quaternion.identity, cellGridParent);
                cellMesh.name = "Cell (" + x + ", " + y + ")";
                CellClass newCell = new CellClass();
                newCell.GridPosition = new Vector2(x, y);
                newCell.CellGO = cellMesh;
                newCell.CellPOIGO = poiMesh;
                //GridOfCells[x, y] = new CellClass(x, y, 1, cellMesh);
                GridOfCells[x, y] = newCell;
            }
        }

        // Set adjacent cells
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                CellClass c = GridOfCells[x, y];
                CellClass adjacentCell;
                if (c.XPosition > 0)
                {
                    adjacentCell = GridOfCells[x - 1, y];
                    c.AdjacentCells.Add(adjacentCell);
                }
                if (c.YPosition > 0)
                {
                    adjacentCell = GridOfCells[x, y - 1];
                    c.AdjacentCells.Add(adjacentCell);
                }
                if (c.XPosition < gridSize - 1)
                {
                    adjacentCell = GridOfCells[x + 1, y];
                    c.AdjacentCells.Add(adjacentCell);
                }
                if (c.YPosition < gridSize - 1)
                {
                    adjacentCell = GridOfCells[x, y + 1];
                    c.AdjacentCells.Add(adjacentCell);
                }
            }
        }
    }

    public void SaveMap( string saveName)
    {
        saveName = input.text;
        SaveSystem.SaveMap(saveName, this);
    }
    public void LoadMap( string saveName)
    {
        saveName = input.text;
        MapData map = SaveSystem.LoadMap(saveName);
        Debug.Log(map.name + " | Did this work?");
        for(int i = 0; i < 25; i++)
        {
            Debug.Log(input.text);
            Debug.Log(map._grid[i,0].cellType);
        }
        ClearMap();
        BuildMap(map);
    }

    public void ClearMap()
    {
        foreach (Transform child in cellGridParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void BuildMap(MapData map)
    {
        GridOfCells = new CellClass[gridSize, gridSize];

        // Create cells
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, y * cellSize);
                GameObject cellMesh = GameObject.Instantiate(cellPrefab, cellPosition, Quaternion.identity, cellGridParent);
                GameObject poiMesh = GameObject.Instantiate(poiPrefab, cellPosition, Quaternion.identity, cellGridParent);
                cellMesh.name = "Cell (" + x + ", " + y + ")";
                CellClass newCell = new CellClass();
                newCell.GridPosition = new Vector2(x, y);
                newCell.CellGO = cellMesh;
                newCell.CellPOIGO = poiMesh;
                GridOfCells[x, y] = newCell;

                switch (map._grid[x,y].cellType)
                {
                    case CellType.Plains:
                        newCell.CellTileType = CellType.Plains;
                        newCell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[0];
                        break;
                    case CellType.Forest:
                        newCell.CellTileType = CellType.Forest;
                        newCell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[1];
                        break;
                    case CellType.Desert:
                        newCell.CellTileType = CellType.Desert;
                        newCell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[2];
                        break;
                    case CellType.River:
                        newCell.CellTileType = CellType.River;
                        newCell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[3];
                        break;
                    case CellType.Wasteland:
                        newCell.CellTileType = CellType.Wasteland;
                        newCell.CellGO.GetComponentInChildren<Renderer>().material = cellMaterials[4];
                        break;
                }

                Renderer[] renderers = newCell.CellGO.GetComponentsInChildren<Renderer>();
                switch (map._grid[x, y].poiType)
                {
                    case POIType.None:
                        newCell.CellPOIType = POIType.None;
                        Debug.Log(renderers[1].material.name);
                        renderers[1].material = poiMaterials[0];
                        break;
                    case POIType.Bunker:
                        newCell.CellPOIType = POIType.Bunker;
                        Debug.Log(renderers[1].material.name);
                        renderers[1].material = poiMaterials[1];
                        break;
                    case POIType.Lero:
                        newCell.CellPOIType = POIType.Lero;
                        Debug.Log(renderers[1].material.name);
                        renderers[1].material = poiMaterials[2];
                        break;
                    case POIType.FallenLeaf:
                        newCell.CellPOIType = POIType.FallenLeaf;
                        Debug.Log(renderers[1].material.name);
                        renderers[1].material = poiMaterials[3];
                        break;
                    case POIType.Onivine:
                        newCell.CellPOIType = POIType.Onivine;
                        Debug.Log(renderers[1].material.name);
                        renderers[1].material = poiMaterials[2];
                        break;
                    case POIType.Ruins:
                        newCell.CellPOIType = POIType.Ruins;
                        Debug.Log(renderers[1].material.name);
                        renderers[1].material = poiMaterials[3];
                        break;
                }

            }
        }
        
    }
}
[System.Serializable]
public enum CellType { Desert, Plains, Forest, Wasteland, Ocean, River }
[System.Serializable]
public enum POIType { None, Bunker, FallenLeaf, Lero, Onivine, Ruins }
