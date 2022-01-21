/*
	Modified NewBehaviorScript by Billy Simson
	OOP design study
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OriginalTactics
{
    // TODO: Set this up for path mesh
    // TODO: Move UI stuff to UIScript
    /*
     * GOALS:
-Edit mode toggle
--clear path graphics
--change cell type
-Algorithm toggle
-graphical path overlay instead of colouring cells

Stretch:
-procedural mesh/texture for 

Databases
-People
-Items
-Cell types


UI/Mouse
-disable mouse
-mouse modes
-Grid MUST be separate! It is a component only
     */
    public enum CellType { Desert, Plains, Forest, Wasteland, Ocean, River }
    public class GridClass : MonoBehaviour
    {
        // Modifying this: Must be self-contained using OOP principles
        // Build/Edit mode: To build or update the grid
        // Show path mode: To show the pathfinding mesh for moving/attacking/etc

        #region Fields
        // Use this region for fields
        [SerializeField]
        private int gridSize = 25;
        [SerializeField]
        private float cellSize = 1.1f;
        [SerializeField]
        private int testMoveDistance = 10;
        [SerializeField]
        private GameObject cellPrefab;
        [SerializeField]
        Transform cellGridParent;
        [SerializeField]
        public CellClass[,] GridOfCells;
        [SerializeField]
        List<CellClass> colouredCells;

        //
        public bool editMode = true;
        public enum PathfinderAlgorithm
        {
            Dijkstra, Astar
        }
        public PathfinderAlgorithm pathAlgorithm = PathfinderAlgorithm.Dijkstra;
        
        public CellType cellType = CellType.Plains;

        #endregion
        #region Behaviour Methods
        // Use this for initialization
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

        }

        // Update is called once per frame
        void Update()
        {
            if (editMode == false)
            {
                //ClickToFindPath();
                //ClickFindPathByAlgorithm();
            }
            else
            {
                //if(Input.GetMouseButtonDown(0))
                    //ClickToEditCellType();
            }
        }

        // Casts a ray against the cells and finds a path
        /*private void ClickToFindPath()
        {
            try
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
                {
                    CellClass cell = CellMeshFromTransform(hit.transform.parent.gameObject);
                    if (cell != null)
                    {
                        //List<CellClass> cells = Pathfinder.GetPath_CellClass(cell, testMoveDistance);
                        List<CellClass> cells = Pathfinder.GetPath_AStar(cell, testMoveDistance);
                        ColourCells(cells);
                        colouredCells = cells;
                    }
                }

            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
		            Application.Quit();
#endif
            }
        }*/
        // Casts a ray against the cells and finds a path
        private CellClass ClickToFindCell()
        {
            CellClass myCell = null;
            try
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
                {
                    CellClass cell = CellMeshFromTransform(hit.transform.parent.gameObject);
                    if (cell != null)
                    {
                        myCell = cell;
                    }
                }
            }

            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
            return myCell;
        }
        /*private void ClickFindPathByAlgorithm()
        {
            // Check pathfinder mode
            CellClass cell = ClickToFindCell();
            List<CellClass> cells;
            if (cell == null)
                return;
            switch (pathAlgorithm)
            {
                case PathfinderAlgorithm.Dijkstra:
                    cells = Pathfinder.GetPath_Dijkstra(cell, testMoveDistance);
                    ColourCells(cells);
                    colouredCells = cells;
                    break;
                case PathfinderAlgorithm.Astar:
                    cells = Pathfinder.GetPath_AStar(cell, testMoveDistance);
                    ColourCells(cells);
                    colouredCells = cells;
                    break;
            }
        }*/
        /*
        private void ClickToEditCellType()
        {
            //throw new NotImplementedException();
            // Change cell type of cell clicked on by mouse
            CellClass cell = ClickToFindCell();
            if (cell == null) return;
            else
            {
                // Change cell or whatever here. Need cell type database
                switch (cellType)
                {
                    case CellType.Plains:
                        cell.CellTileType = CellType.Plains;
                        break;
                    case CellType.Forest:
                        cell.CellTileType = CellType.Forest;
                        break;
                    case CellType.River:
                        cell.CellTileType = CellType.River;
                        break;
                }
            }
        }*/

        //TODO: Shift this to the path mesh
        // Clears all coloured cells then coloured cells in a given list
        private void ColourCells(List<CellClass> cells)
        {
            ClearCells(colouredCells);
            foreach (CellClass c in cells)
            {
                c.CellGO.GetComponentInChildren<Renderer>().material.color = Color.blue;
            }
        }
        //Clear cell method separate from ColourCells
        private void ClearCells(List<CellClass> cells)
        {
            foreach (CellClass c in cells)
            {
                c.CellGO.GetComponentInChildren<Renderer>().material.color = Color.white;
            }
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
                    cellMesh.name = "Cell (" + x + ", " + y + ")";
                    //GridOfCells[x, y] = new CellClass(x, y, 1, cellMesh);
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
        #endregion
    }
}