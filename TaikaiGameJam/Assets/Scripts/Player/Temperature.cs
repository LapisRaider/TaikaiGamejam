[System.Serializable]
public class Temperature
{
    //temperature is affected based on the number of trees and plants u have
    public int[] m_TreeToTemperature = new int[(int)TemperatureType.ALL_TYPES];
    public TemperatureType m_CurrTempType = TemperatureType.EXTREMELY_HOT;

    public void UpdateTemperature(int treeNumber)
    {
        //based on plant and flower count
        if (m_CurrTempType == TemperatureType.NICE)
            return;

        if (treeNumber > m_TreeToTemperature[(int)m_CurrTempType + 1])
        {
            m_CurrTempType += 1; //go to next temperature type
            //TODO:: update temperature UI
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
