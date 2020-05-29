using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomEventManager : SingletonBase<RandomEventManager>
{
    public RandomEvent[] m_PositiveEvents;
    public RandomEvent[] m_NegativeEvents;

    public float m_NegativeEventChance = 0.5f;
    int m_TotalPositiveWeightage = 0;
    int m_TotalNegativeWeightage = 0;

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

        int currentWeightage = 0;
        foreach(RandomEvent eventObj in m_PositiveEvents)
        {
            eventObj.m_Weightage.x = currentWeightage;
            eventObj.m_Weightage.y = currentWeightage + eventObj.m_Chance;
            currentWeightage += eventObj.m_Chance;
        }
        m_TotalPositiveWeightage = currentWeightage;

        //---------------------For the negative events------------------------------
        for (int i = 0; i < m_NegativeEvents.Length; ++i)
        {
            bool sorted = true;
            for (int r = i; r < m_NegativeEvents.Length - i; ++r)
            {
                if (m_NegativeEvents[i].m_Chance > m_NegativeEvents[r].m_Chance)
                {
                    //swap
                    RandomEvent temp = m_NegativeEvents[i];
                    m_NegativeEvents[i] = m_NegativeEvents[r];
                    m_NegativeEvents[r] = temp;
                    sorted = false;
                }
            }

            if (sorted)
                break;
        }

        currentWeightage = 0;
        foreach (RandomEvent eventObj in m_NegativeEvents)
        {
            eventObj.m_Weightage.x = currentWeightage;
            eventObj.m_Weightage.y = currentWeightage + eventObj.m_Chance;
            currentWeightage += eventObj.m_Chance;
        }
        m_TotalNegativeWeightage = currentWeightage;
    }

    public void StartEvent()
    {
        //randomize the weightage, loop through the array to see if theres any within the correct values

        //TODO:: check one time only events
        float negativeChance = Random.Range(0.0f, 1.0f);
        if (negativeChance <= m_NegativeEventChance) //negative event
        {
            int weight = Random.Range(0, m_TotalNegativeWeightage);
            foreach (RandomEvent randomEvent in m_NegativeEvents)
            {
                if (weight > randomEvent.m_Weightage.x &&
                    weight <= randomEvent.m_Weightage.y)
                {
                    HandleNegativeEvent(randomEvent.m_EventScritableObj);
                    break;
                }
            }
        }
        else
        {
            int weight = Random.Range(0, m_TotalPositiveWeightage);
            foreach (RandomEvent randomEvent in m_PositiveEvents)
            {
                if (weight > randomEvent.m_Weightage.x &&
                    weight <= randomEvent.m_Weightage.y)
                {
                    HandlePositiveEvents(randomEvent.m_EventScritableObj);
                    break;
                }
            }
        }
    }

    public void StartEvent(EventScriptableObj eventObj, bool positive = true)
    {
        if (!positive)
            HandleNegativeEvent(eventObj);
        else
            HandlePositiveEvents(eventObj);
    }

    public void UpdateUI(EventScriptableObj eventObj)
    {
        if (m_UIGameObject == null)
            return;

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

    [HideInInspector] public Vector2Int m_Weightage = Vector2Int.zero;
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


