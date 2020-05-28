﻿using UnityEngine;

public class Popularity
{
    [Header("Plants popularity rate")]
    public float m_TreePopularityRate = 0.1f;
    public float m_PlantPopularityRate = 0.1f;

    [Header("Increase Rate")]
    public float m_IncreaseTimeRate = 0.1f;
    public int m_IncreaseNumber = 2;
    float m_IncreaseTimeTracker = 0.0f;

    [HideInInspector] public int m_CurrentPopularity = 0;
    [HideInInspector] public int m_NextPopularity = 0;

    public void Init()
    {
        UpdatePopularity();
        m_IncreaseTimeTracker = 0.0f;

        //set UI stuff
        UIManager.Instance.SetPopularityUI(m_CurrentPopularity);
    }

    public void Update()
    {
        //so the popularity wont look it randomly jumped
        if (m_CurrentPopularity != m_NextPopularity)
        {
            m_IncreaseTimeTracker += Time.deltaTime;

            if (m_IncreaseTimeTracker > m_IncreaseTimeRate)
            {
                m_CurrentPopularity += m_IncreaseNumber;
                m_CurrentPopularity = Mathf.Min(m_CurrentPopularity, m_NextPopularity);
                m_IncreaseTimeTracker = 0.0f;

                //UPDATE UI
                UIManager.Instance.SetPopularityUI(m_CurrentPopularity);
            }
        }
    }

    public void UpdatePopularity()
    {
        //e^tree * percentage
        float powerFromTrees = m_TreePopularityRate * GameStats.Instance.m_CurrentTreeNumber
            + m_PlantPopularityRate * GameStats.Instance.m_CurrentPlantNumber;

        m_NextPopularity = (int)(Mathf.Exp(powerFromTrees) * GameStats.Instance.m_RecordingEquipmentStats.GetPopularityRate());
    }
}