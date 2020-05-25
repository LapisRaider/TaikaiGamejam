using System.Collections.Generic;
using UnityEngine;

public class PlantDataBase : SingletonBase<PlantDataBase>
{
    [Header("Prefabs")]
    public GameObject m_TreePrefab;
    public GameObject m_PlantPrefab;

    [Header("ScriptableObjects")]
    public List<PlantScriptableObj> m_PlantScriptableObjList = new List<PlantScriptableObj>();
    public Dictionary<Plant_Types, PlantScriptableObj> m_PlantDictionary = new Dictionary<Plant_Types, PlantScriptableObj>();

    void Awake()
    {
        //sort it out nicely
        foreach (PlantScriptableObj plantData in m_PlantScriptableObjList)
        {
            m_PlantDictionary.Add(plantData.m_PlantType, plantData);
        }
    }
}

public enum Plant_Types
{
    TREES,
    FLOWERS,
}

