using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : SingletonBase<MapManager>
{
    [Header("MapInfo")]
    public TilemapRenderer m_TileMapRenderer;
    public Tilemap m_TileMap;
    [HideInInspector] public Bounds m_MapBoundary;
    [HideInInspector] public BoundsInt m_MapBoundaryGridNo;

    public Grid m_MapGrid;
    [HideInInspector] public Vector2 m_GridSize;

    [Header("Plants Info")]
    [HideInInspector] public Dictionary<Vector2Int, PlantTree> m_TreeOnMap = new Dictionary<Vector2Int, PlantTree>();
    [HideInInspector] public Dictionary<Vector2Int, Plant> m_PlantOnMap = new Dictionary<Vector2Int, Plant>();

    public PlantObjectPooler m_PlantManager = new PlantObjectPooler();

    public void Awake()
    {
        if (m_TileMapRenderer != null)
            m_MapBoundary = m_TileMapRenderer.bounds;

        if (m_TileMap != null)
            m_MapBoundaryGridNo = m_TileMap.cellBounds;

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
        if (m_TreeOnMap.ContainsKey(tilePos))
            return false;

        if (m_PlantOnMap.ContainsKey(tilePos))
            return false;

        //TODO:: MAKRE SURE TREE IS IN BOUNDS
        //TODO:: TAKE NOTE OF TREE SURROUNDING SPACE

        return true;
    }

    public float Plant(Vector2Int tilePos)
    {
        //gets a random available plant type
        Inventory inventory = GameStats.Instance.m_Inventory;
        Plant_Types type = GameStats.Instance.GetAvilablePlantType();
        bool isTree = type < Plant_Types.FLOWERS;

        if (isTree)
        {
            if (m_TreeOnMap.ContainsKey(tilePos))
                return 0.0f;

            //plant tree
            GameObject treeObj = m_PlantManager.GetAndSpawnTree();
            if (treeObj == null)
                return 0.0f;

            treeObj.transform.position = m_MapGrid.GetCellCenterWorld((Vector3Int)tilePos);
            treeObj.SetActive(true);

            PlantTree plantTree = treeObj.GetComponent<PlantTree>();
            if (plantTree)
            {
                m_PlantManager.InitType(type, tilePos, plantTree);
                m_TreeOnMap.Add(tilePos, plantTree);

                GameStats.Instance.UpdateCurrentPlantNumber(isTree, m_TreeOnMap.Count);

                inventory.RemoveOneFromInventory(type); //remove one from inventory

                return plantTree.m_PlantTime;
            }
        }
        else
        {
            //TODO:: plant other stuff
            GameStats.Instance.UpdateCurrentPlantNumber(isTree, m_PlantOnMap.Count);
        }


        return 1.0f;
    }

    public void RemoveTree(Vector2Int tilePos)
    {
        if (!m_TreeOnMap.ContainsKey(tilePos))
            return;

        m_TreeOnMap.Remove(tilePos);
        GameStats.Instance.UpdateCurrentPlantNumber(true, m_TreeOnMap.Count);
    }
    #endregion

    //TODO:: call this when temperature is updated
    public void PlantsUpdateTemperature()
    {

    }

    public PlantDataBase GetPlantDataBase()
    {
        return m_PlantManager.m_PlantDataBase;
    }
}
