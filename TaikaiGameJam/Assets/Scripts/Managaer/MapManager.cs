using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : SingletonBase<MapManager>
{
    [Header("MapInfo")]
    public TilemapRenderer m_TileMap;
    [HideInInspector] public Vector2Int m_MapDimensions;
    [HideInInspector] public Bounds m_MapBoundary;

    public Grid m_MapGrid;
    [HideInInspector] public Vector2 m_GridSize;

    [Header("Plants Info")]
    Dictionary<Vector2Int, PlantTree> m_TreeOnMap = new Dictionary<Vector2Int, PlantTree>();
    public Transform m_TreeParent;

    public void Awake()
    {
        if (m_TileMap != null)
        {
            m_MapBoundary = m_TileMap.bounds;
            //m_MapDimensions = (Vector2Int)m_TileMap.size;
            //m_MapBoundary = m_TileMap.GetBoundsLocal();
        }

        if (m_MapGrid != null)
            m_GridSize = (Vector2)m_MapGrid.cellSize;
    }

    public Vector2Int GetWorldToGridPos(Vector2 pos)
    {
        if (m_MapGrid != null)
            return (Vector2Int)m_MapGrid.WorldToCell(pos);

        return Vector2Int.zero;
    }

    public Vector2 GetGridPosToWorld(Vector2Int pos)
    {
        if (m_MapGrid != null)
            return m_MapGrid.CellToWorld((Vector3Int)pos);

        return Vector2.zero;
    }

    #region PlantTrees
    public bool CheckCanPlantTree(Vector2Int tilePos)
    {


        return true;
    }

    public bool Plant(Vector2Int tilePos, Plant_Types type, bool isTree = true)
    {
        if (isTree)
        {
            if (m_TreeOnMap.ContainsKey(tilePos))
                return false;

            //plant tree
            GameObject treeObj = Instantiate(PlantDataBase.Instance.m_PlantPrefab);
            if (treeObj == null)
                return false;

            treeObj.transform.parent = m_TreeParent;
            treeObj.transform.position = m_MapGrid.CellToWorld((Vector3Int)tilePos);

            PlantTree plantTree = treeObj.GetComponent<PlantTree>();
            if (plantTree)
            {
                plantTree.Init(PlantDataBase.Instance.m_PlantDictionary[type]);
                m_TreeOnMap.Add(tilePos, plantTree);
            }
        }
        else
        {
            //plant other stuff
        }

        return true;
    }
    #endregion
}
