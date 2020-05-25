using UnityEngine;

public class Plant : MonoBehaviour
{
    [HideInInspector] public float m_GrowthTime = 1.0f;
    [HideInInspector] public float m_TemperatureToGrow;
    [HideInInspector] public bool m_Grown = false;

    public SpriteRenderer m_SpriteRenderer;

    public void Init(PlantScriptableObj plantScript)
    {
        m_GrowthTime = plantScript.m_GrowthTime;
        m_TemperatureToGrow = plantScript.m_TemperatureToGrow;

        //set sprite stuff
        m_SpriteRenderer.sprite = plantScript.m_PlantSprite;
        m_SpriteRenderer.sortingOrder = (int)(transform.position.y * -100);

        //transform.localScale = Vector3.zero;
    }

    public void Grow()
    {
        //transform.localScale = Tran
    }

    public void TemperatureIncrease()
    {
        //check if plant can still survive when the temperature increased
        //have a callback to handle this
    }
}
