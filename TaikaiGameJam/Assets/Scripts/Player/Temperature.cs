[System.Serializable]
public class Temperature
{
    //temperature is affected based on the number of trees and plants u have
    public int[] m_TreeToTemperature = new int[(int)TemperatureType.ALL_TYPES];
    public float m_TemperaturePerTree = 0.5f;
    public float m_TemperaturePerPlant = 0.1f;

    public TemperatureType m_CurrTempType = TemperatureType.EXTREMELY_HOT;

    float m_CurrTemperatureAmt = 0.0f;
    float m_TemperatureOffset = 0.0f;

    public void Init()
    {
        UIManager.Instance.SetTemperatureUI(m_CurrTempType);
    }

    public void UpdateTemperature(int treeNumber, int plantNumber)
    {
        m_CurrTemperatureAmt = treeNumber * m_TemperaturePerTree + plantNumber * m_TemperaturePerPlant;
        m_CurrTemperatureAmt += m_TemperatureOffset;

        //based on plant and flower count
        if (m_CurrTemperatureAmt > m_TreeToTemperature[(int)m_CurrTempType])
        {
            m_CurrTempType += 1; //go to next temperature type
            if (m_CurrTempType > TemperatureType.NICE)
            {
                m_CurrTempType = TemperatureType.NICE;
            }
            else
            {
                MapManager.Instance.AllPlantsTreesUpdateTemperature();
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
                MapManager.Instance.AllPlantsTreesUpdateTemperature();
            }

            UIManager.Instance.SetTemperatureUI(m_CurrTempType);
        }
    }

    public void AddToTemperatureOffset(float offset)
    {
        m_TemperatureOffset += offset;
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
