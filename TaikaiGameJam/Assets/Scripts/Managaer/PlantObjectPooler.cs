using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantObjectPooler
{
    public PlantDataBase m_PlantDataBase = new PlantDataBase();

    [Header("Tree spawn info")]
    public int m_InitialTreeSpawnObjPooler = 10;
    public int m_MaxTreeSpawnObjPooler = 100;
    public Transform m_TreeParent;

    [Header("Plant spawn info")]
    public int m_InitialPlantSpawnObjPooler = 10;
    public int m_MaxPlantSpawnObjPooler = 10;
    public Transform m_PlantParent;

    List<GameObject> m_TreeList = new List<GameObject>();
    List<GameObject> m_PlantList = new List<GameObject>();

    public void Init()
    {
        m_PlantDataBase.Init();
        SpawnTrees(m_InitialTreeSpawnObjPooler);
        SpawnPlant(m_InitialPlantSpawnObjPooler);
    }

    public void InitType(Plant_Types type, Vector2Int gridPos, Plant plant)
    {
        if (!m_PlantDataBase.m_PlantDictionary.ContainsKey(type))
            return;

        plant.Init(m_PlantDataBase.m_PlantDictionary[type], gridPos);
    }

    #region Tree
    public GameObject GetAndSpawnTree()
    {
        foreach (GameObject tree in m_TreeList)
        {
            if (tree.activeSelf)
                continue;

            return tree;
        }

        SpawnTrees(m_InitialTreeSpawnObjPooler);

        return GetAndSpawnTree();
    }

    public void SpawnTrees(int number)
    {
        for (int i = 0; i < number; ++i)
        {
            GameObject treeObj = GameObject.Instantiate(m_PlantDataBase.m_TreePrefab);
            treeObj.transform.parent = m_TreeParent;
            treeObj.SetActive(false);
            m_TreeList.Add(treeObj);
        }
    }
    #endregion

    #region Plants
    public GameObject GetAndSpawnPlant()
    {
        foreach (GameObject plant in m_PlantList)
        {
            if (plant.activeSelf)
                continue;

            return plant;
        }

        SpawnPlant(m_InitialPlantSpawnObjPooler);

        return GetAndSpawnTree();
    }

    public void SpawnPlant(int number)
    {
        for (int i = 0; i < number; ++i)
        {
            GameObject plantObj = GameObject.Instantiate(m_PlantDataBase.m_PlantPrefab);
            plantObj.transform.parent = m_PlantParent;
            plantObj.SetActive(false);
            m_PlantList.Add(plantObj);
        }
    }
    #endregion
}
