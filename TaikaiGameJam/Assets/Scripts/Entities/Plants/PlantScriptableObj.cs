using UnityEngine;

[CreateAssetMenu(menuName = "Plants")]
public class PlantScriptableObj : ScriptableObject
{
    [Header("Plant Details")]
    public string m_PlantName = "Tree";
    public string m_PlantDescription = "Provides oxygen";
    public Sprite m_PlantSprite;
    public Plant_Types m_PlantType;

    [Header("Plant Growing Details")]
    public float m_GrowthTime = 1.0f;
    public float m_PlantTime = 1.0f;
    public TemperatureType m_TemperatureToGrow = TemperatureType.EXTREMELY_HOT;
    public int m_PlantHealth = 5;

    [Header("Plant Info")]
    public int m_Price = 50;

    [Header("Number to plant")]
    public int m_MaxNumberToPlant = 10;
}
