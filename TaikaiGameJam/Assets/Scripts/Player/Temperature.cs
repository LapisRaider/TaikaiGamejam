﻿using UnityEngine;

[System.Serializable]
public class Temperature
{
    //temperature is affected based on the number of trees and plants u have
    public int[] m_TreeToTemperature = new int[(int)TemperatureType.ALL_TYPES];
    public TemperatureType m_CurrTempType = TemperatureType.EXTREMELY_HOT;

    //TODO:: fix ALGORITHM FOR THIS

    public void Init()
    {
        UIManager.Instance.SetTemperatureUI(m_CurrTempType);
    }

    public void UpdateTemperature(int treeNumber)
    {
        //based on plant and flower count
        if (treeNumber > m_TreeToTemperature[(int)m_CurrTempType])
        {
            m_CurrTempType += 1; //go to next temperature type
            if (m_CurrTempType > TemperatureType.NICE)
            {
                m_CurrTempType = TemperatureType.NICE;
            }
            else
            {
                //TODO:: UPDATE ALL TREES

            }

            UIManager.Instance.SetTemperatureUI(m_CurrTempType);
        }
        else
        {
            m_CurrTempType -= 1;
            if (m_CurrTempType < 0)
            {
                m_CurrTempType = TemperatureType.EXTREMELY_HOT;
            }
            else
            {
                //TODO:: UPDATE ALL TREES

            }

            UIManager.Instance.SetTemperatureUI(m_CurrTempType);
        }
    }
}

public enum TemperatureType
{
    EXTREMELY_HOT,
    VERY_HOT,
    HOT,
    NICE,
    ALL_TYPES
}
