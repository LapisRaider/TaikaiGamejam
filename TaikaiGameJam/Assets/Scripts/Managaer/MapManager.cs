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
    [HideInInspector] public Dictionary<Vector2Int, PlantTree> m_TreeOnMap = new Dictionary<Vector2Int, PlantTree>();
    public PlantObjectPooler m_PlantManager = new PlantObjectPooler();

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

        m_PlantManager.Init();
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
            return m_MapGrid.GetCellCenterWorld((Vector3Int)pos);

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
            GameObject treeObj = m_PlantManager.GetAndSpawnTree();
            if (treeObj == null)
                return false;

            treeObj.transform.position = m_MapGrid.GetCellCenterWorld((Vector3Int)tilePos);
            treeObj.SetActive(true);

            PlantTree plantTree = treeObj.GetComponent<PlantTree>();
            if (plantTree)
            {
                m_PlantManager.InitType(type, tilePos, plantTree);
                m_TreeOnMap.Add(tilePos, plantTree);
            }
        }
        else
        {
            //plant other stuff
        }

        return true;
    }

    public void RemoveTree(Vector2Int tilePos)
    {
        if (!m_TreeOnMap.ContainsKey(tilePos))
            return;

        m_TreeOnMap.Remove(tilePos);
    }
    #endregion
}
