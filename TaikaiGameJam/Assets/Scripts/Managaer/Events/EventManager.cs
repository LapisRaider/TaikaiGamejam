using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomEventManager : MonoBehaviour
{
    public RandomEvent[] m_PositiveEvents;
    public RandomEvent[] m_NegativeEvents;

    public float m_NegativeEventChance = 0.5f;
    int m_TotalWeightage = 0;

    [Header("UI")]
    public GameObject m_UIGameObject;
    public TextMeshProUGUI m_DescriptionText;
    public TextMeshProUGUI m_NameOfPersonInvolveText;
    public TextMeshProUGUI m_TitleText;
    public TextMeshProUGUI m_AffectedText; //how the player is affected
    public Image m_Image;

    public void Awake()
    {
        //arrange the positive and negative event based on weightage
        for (int i = 0; i < m_PositiveEvents.Length; ++i)
        {
            bool sorted = true;
            for (int r = i; r < m_PositiveEvents.Length - i; ++r)
            {
                if (m_PositiveEvents[i].m_Chance > m_PositiveEvents[r].m_Chance)
                {
                    //swap
                    RandomEvent temp = m_PositiveEvents[i];
                    m_PositiveEvents[i] = m_PositiveEvents[r];
                    m_PositiveEvents[r] = temp;
                    sorted = false;
                }
            }

            if (sorted)
                break;
        }

        int m_CurrentWeightage = 0;
        foreach(RandomEvent eventObj in m_PositiveEvents)
        {
            eventObj.m_Weightage = m_CurrentWeightage + eventObj.m_Chance;
            m_CurrentWeightage += eventObj.m_Chance;
        }
    }

    public void StartEvent(EventScriptableObj eventObj)
    {
        //TODO:: randomized
        //TODO:: check one time only events
        float negativeChance = Random.Range(0.0f, 1.0f);
        if (negativeChance <= m_NegativeEventChance) //negative event
        {
            HandleNegativeEvent(eventObj);
        }
        else
        {
            HandlePositiveEvents(eventObj);
        }
    }

    public void UpdateUI(EventScriptableObj eventObj)
    {
        //randomize 
        if (eventObj.m_EventPicture.Length > 0)
        {
            m_Image.sprite = eventObj.m_EventPicture[Random.Range(0, eventObj.m_EventPicture.Length)];
        }

        if (eventObj.m_Description.Length > 0)
        {
            m_DescriptionText.text = eventObj.m_Description[Random.Range(0, eventObj.m_Description.Length)];
        }

        if (eventObj.m_Title.Length > 0)
        {
            m_TitleText.text = eventObj.m_Title[Random.Range(0, eventObj.m_Title.Length)];
        }

        if (eventObj.m_NameOfPersonInvolve.Length > 0)
        {
            m_NameOfPersonInvolveText.text = eventObj.m_NameOfPersonInvolve[Random.Range(0, eventObj.m_NameOfPersonInvolve.Length)];
        }


        //TODO:: do the affected text

        m_UIGameObject.SetActive(true);
    }

    public void HandlePositiveEvents(EventScriptableObj eventObj)
    {
        switch(eventObj.m_PositiveEventType)
        {
            case PositiveEventTypes.DECREASE_TEMPERATURE:
                {
                    //TODO:: decrease temperature
                }
                break;
            case PositiveEventTypes.GET_MONEY:
                {

                }
                break;
            case PositiveEventTypes.INCREASE_POPULARITY:
                {

                }
                break;
        }
    }

    public void HandleNegativeEvent(EventScriptableObj eventObj)
    {
        switch (eventObj.m_NegativeEventType)
        {
            case NegativeEventTypes.INCREASE_TEMPERATURE:
                //TODO:: increase temperature
                break;
            case NegativeEventTypes.DECREASE_POPULARITY:

                break;
            case NegativeEventTypes.LOSE_MONEY:

                break;
        }
    }
}

[System.Serializable]
public class RandomEvent
{
    public EventScriptableObj m_EventScritableObj;
    public int m_Chance = 0;

    [HideInInspector] public int m_Weightage = 0;
    [HideInInspector] public bool m_EventCompleted = false;
}

public enum PositiveEventTypes
{
    P_NONE,
    DECREASE_TEMPERATURE,
    INCREASE_POPULARITY,
    GET_MONEY,
}

public enum NegativeEventTypes
{
    N_NONE,
    INCREASE_TEMPERATURE,
    DECREASE_POPULARITY,
    LOSE_MONEY
}


