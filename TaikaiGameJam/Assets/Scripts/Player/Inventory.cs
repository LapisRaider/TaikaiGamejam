using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [HideInInspector] public Dictionary<Plant_Types, int> m_PlantInventory = new Dictionary<Plant_Types, int>();

    public void BuyOneIntoInventory(Plant_Types plantType)
    {
        if (m_PlantInventory.ContainsKey(plantType))
        {
            m_PlantInventory[plantType] += 1;
            return;
        }

        //if not add it into inventory
        m_PlantInventory.Add(plantType, 1);
    }

    //check if there are any available in the inventory
    public bool HaveInInventory(Plant_Types plantType)
    {
        return (m_PlantInventory.ContainsKey(plantType));
    }

    public bool RemoveOneFromInventory(Plant_Types plantType)
    {
        if (!m_PlantInventory.ContainsKey(plantType))
            return false;

        //remove it from the inventory if there arent anymore
        m_PlantInventory[plantType] -= 1;
        if (m_PlantInventory[plantType] <= 0)
            m_PlantInventory.Remove(plantType);

        return true;
    }

    public Plant_Types GetAnyAvailablePlantType()
    {
        int randomRange = Random.Range(0, m_PlantInventory.Count);

        return m_PlantInventory.ElementAt(randomRange).Key;
    }

    public int GetAmtInInventory(Plant_Types plantType)
    {
        //dont have in inventory
        if (!m_PlantInventory.ContainsKey(plantType))
            return 0;

        return m_PlantInventory[plantType];
    }
}
