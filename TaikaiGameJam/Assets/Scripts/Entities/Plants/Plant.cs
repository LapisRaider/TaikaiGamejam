using UnityEngine;

public class Plant : MonoBehaviour
{
    [HideInInspector] public float m_GrowthTime = 1.0f;
    [HideInInspector] public float m_PlantTime = 1.0f;
    [HideInInspector] public TemperatureType m_TemperatureToGrow;
    [HideInInspector] public bool m_Grown = false;
    [HideInInspector] public int m_CurrentHealth = 0;
    [HideInInspector] public Vector2Int m_PlantGridPos = Vector2Int.zero;

    public SpriteRenderer m_SpriteRenderer;

    public void Init(PlantScriptableObj plantScript, Vector2Int plantGridPos)
    {
        m_GrowthTime = plantScript.m_GrowthTime;
        m_PlantTime = plantScript.m_PlantTime;
        m_TemperatureToGrow = plantScript.m_TemperatureToGrow;
        m_CurrentHealth = plantScript.m_PlantHealth;

        m_PlantGridPos = plantGridPos;

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

    public void RemoveHealth(int healthDeduct)
    {
        m_CurrentHealth -= healthDeduct;
        if (m_CurrentHealth <= 0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {
        gameObject.SetActive(false);
        return;
    }
}
