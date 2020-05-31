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
    public int m_PlantSpaceToCheck = 2;
    public int m_BoundaryOffset = 2;

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

        //MAKRE SURE TREE IS IN BOUNDS
        if (tilePos.x >= m_MapBoundaryGridNo.xMax || tilePos.x <= m_MapBoundaryGridNo.xMin 
            || tilePos.y >= m_MapBoundaryGridNo.yMax - m_BoundaryOffset || tilePos.y <= m_MapBoundaryGridNo.yMin)
        {
            return false;
        }

        //check if got space
        for (int row = -m_PlantSpaceToCheck; row <= m_PlantSpaceToCheck; ++row)
        {
            for (int col = -m_PlantSpaceToCheck; col <= m_PlantSpaceToCheck; ++col)
            {
                Vector2Int nextTile = new Vector2Int(col, row) + tilePos;
                if (m_TreeOnMap.ContainsKey(nextTile))
                    return false;

                if (m_PlantOnMap.ContainsKey(nextTile))
                    return false;
            }
        }

        return true;
    }

    public float Plant(Vector2Int tilePos)
    {
        //gets a random available plant type
        Inventory inventory = GameStats.Instance.m_Inventory;
        Plant_Types type = GameStats.Instance.GetAvilablePlantType();
        bool isTree = type < Plant_Types.FLOWERS;

        if (!inventory.HaveInInventory(type))
            return 0.0f;

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

                inventory.RemoveOneFromInventory(type); //remove one from inventory
                GameStats.Instance.UpdateCurrentPlantNumber(isTree, m_TreeOnMap.Count);

                return plantTree.m_PlantTime;
            }
        }
        else
        {
            if (m_PlantOnMap.ContainsKey(tilePos))
                return 0.0f;

            //plant plants
            GameObject plantObj = m_PlantManager.GetAndSpawnPlant();
            if (plantObj == null)
                return 0.0f;

            plantObj.transform.position = m_MapGrid.GetCellCenterWorld((Vector3Int)tilePos);
            plantObj.SetActive(true);

            Plant plantedPlant = plantObj.GetComponent<Plant>();
            if (plantedPlant)
            {
                m_PlantManager.InitType(type, tilePos, plantedPlant);
                m_PlantOnMap.Add(tilePos, plantedPlant);

                inventory.RemoveOneFromInventory(type); //remove one from inventory
                GameStats.Instance.UpdateCurrentPlantNumber(isTree, m_PlantOnMap.Count);

                return plantedPlant.m_PlantTime;
            }
        }

        return 0.0f;
    }

    public void RemoveTree(Vector2Int tilePos)
    {
        if (!m_TreeOnMap.ContainsKey(tilePos))
            return;

        m_TreeOnMap.Remove(tilePos);
        GameStats.Instance.UpdateCurrentPlantNumber(true, m_TreeOnMap.Count);
    }
    #endregion

    public int GetAmtOfPlantOnMap(Plant_Types plantType)
    {
        if (plantType < Plant_Types.FLOWERS)
            return GetAmtOfTreeTypeOnMap(plantType);

        return GetAmtOfPlantTypeOnMap(plantType);
    }

    public int GetAmtOfTreeTypeOnMap(Plant_Types plantType)
    {
        int amt = 0;
        foreach(KeyValuePair<Vector2Int, PlantTree> tree in m_TreeOnMap)
        {
            if (tree.Value == null)
                continue;

            if (tree.Value.m_PlantType == plantType)
                ++amt;
        }

        return amt;
    }

    public int GetAmtOfPlantTypeOnMap(Plant_Types plantType)
    {
        int amt = 0;
        foreach (KeyValuePair<Vector2Int, Plant> tree in m_PlantOnMap)
        {
            if (tree.Value == null)
                continue;

            if (tree.Value.m_PlantType == plantType)
                ++amt;
        }

        return amt;
    }

    public int GetTotalAmtOfTreeOnMap()
    {
        return m_TreeOnMap.Count;
    }


    public void AllPlantsTreesUpdateTemperature()
    {
        Temperature temperature = GameStats.Instance.m_Temperature;
        if (temperature == null)
            return;

        foreach(KeyValuePair<Vector2Int, Plant> plant in m_PlantOnMap)
        {
            if (plant.Value == null)
                continue;

            plant.Value.CheckTemperatureUpdate(temperature.m_CurrTempType);
        }

        foreach (KeyValuePair<Vector2Int, PlantTree> tree in m_TreeOnMap)
        {
            if (tree.Value == null)
                continue;

            tree.Value.CheckTemperatureUpdate(temperature.m_CurrTempType);
        }
    }

    public PlantDataBase GetPlantDataBase()
    {
        return m_PlantManager.m_PlantDataBase;
    }
}
