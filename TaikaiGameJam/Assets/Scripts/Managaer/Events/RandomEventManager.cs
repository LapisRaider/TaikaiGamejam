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

    public void Update()
    {
        //TODO:: remove this shit
        if (Input.GetKey("up"))
            StartEvent();
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

        //TODO:: INVOLVE LAWYERS
        //TODO:: temp pause game
    }

    #region UI
    public void UpdateUI(EventScriptableObj eventObj, string number)
    {
        if (m_UIGameObject == null)
            return;

        //randomize 
        if (eventObj.m_EventPicture.Length > 0)
            m_Image.sprite = eventObj.m_EventPicture[Random.Range(0, eventObj.m_EventPicture.Length)];

        if (eventObj.m_Description.Length > 0)
            m_DescriptionText.text = eventObj.m_Description[Random.Range(0, eventObj.m_Description.Length)];

        if (eventObj.m_Title.Length > 0)
            m_TitleText.text = eventObj.m_Title[Random.Range(0, eventObj.m_Title.Length)];

        if (eventObj.m_NameOfPersonInvolve.Length > 0)
            m_NameOfPersonInvolveText.text = eventObj.m_NameOfPersonInvolve[Random.Range(0, eventObj.m_NameOfPersonInvolve.Length)];

        switch(eventObj.m_PositiveEventType)
        {
            case PositiveEventTypes.DECREASE_TEMPERATURE:
                m_AffectedText.text = "Temperature decrease by " + number;
                break;
            case PositiveEventTypes.INCREASE_POPULARITY:
                m_AffectedText.text = "Popularity increase by " + number;
                break;
            case PositiveEventTypes.GET_MONEY:
                m_AffectedText.text = "You earn " + number;
                break;
        }

        switch (eventObj.m_NegativeEventType)
        {
            case NegativeEventTypes.INCREASE_TEMPERATURE:
                m_AffectedText.text = "Temperature increase by " + number;
                break;
            case NegativeEventTypes.DECREASE_POPULARITY:
                m_AffectedText.text = "Popularity decrease by " + number;
                break;
            case NegativeEventTypes.LOSE_MONEY:
                m_AffectedText.text = "You lost " + number;
                break;
            case NegativeEventTypes.EVIL_COOPERATE:
                m_AffectedText.text = "They sent people to cut down your trees";
                break;
        }

        m_UIGameObject.SetActive(true);
    }

    public void CloseEventPage()
    {
        m_UIGameObject.SetActive(false);
    }

    #endregion

    public void HandlePositiveEvents(EventScriptableObj eventObj)
    {
        string numberText = "";
        switch (eventObj.m_PositiveEventType)
        {
            case PositiveEventTypes.DECREASE_TEMPERATURE:
                {
                    //TODO:: decrease temperature
                }
                break;
            case PositiveEventTypes.GET_MONEY:
                {
                    Money money = GameStats.Instance.m_Money;
                    if (money == null)
                        return;

                    int increaseAmt = 0;
                    if (eventObj.m_AffectedByPercentage)
                        increaseAmt = (int)((float)money.m_CurrMoney * Random.Range(eventObj.m_MinMaxAffectedPercentages.x, eventObj.m_MinMaxAffectedPercentages.y));
                    else
                        increaseAmt = (int)Random.Range(eventObj.m_MinMaxAffectedAmount.x, eventObj.m_MinMaxAffectedAmount.y);

                    numberText = "$" + increaseAmt.ToString();
                    money.IncreaseMoney(increaseAmt);
                }
                break;
            case PositiveEventTypes.INCREASE_POPULARITY:
                {
                    Popularity popularity = GameStats.Instance.m_Popularity;
                    if (popularity == null)
                        return;

                    int increaseAmt = 0;
                    if (eventObj.m_AffectedByPercentage)
                        increaseAmt = (int)((float)popularity.m_CurrentPopularity * Random.Range(eventObj.m_MinMaxAffectedPercentages.x, eventObj.m_MinMaxAffectedPercentages.y));
                    else
                        increaseAmt = (int)Random.Range(eventObj.m_MinMaxAffectedAmount.x, eventObj.m_MinMaxAffectedAmount.y);

                    numberText = increaseAmt.ToString();
                    popularity.UpdatePopularityOffset(increaseAmt);
                }
                break;
        }

        UpdateUI(eventObj, numberText);
    }

    public void HandleNegativeEvent(EventScriptableObj eventObj)
    {
        string numberText = "";
        switch (eventObj.m_NegativeEventType)
        {
            case NegativeEventTypes.INCREASE_TEMPERATURE:
                {
                    //TODO:: increase temperature

                }
                break;
            case NegativeEventTypes.DECREASE_POPULARITY:
                {
                    Popularity popularity = GameStats.Instance.m_Popularity;
                    if (popularity == null)
                        return;

                    int decreaseAmt = 0;
                    if (eventObj.m_AffectedByPercentage)
                        decreaseAmt = (int)((float)popularity.m_CurrentPopularity * Random.Range(eventObj.m_MinMaxAffectedPercentages.x, eventObj.m_MinMaxAffectedPercentages.y));
                    else
                        decreaseAmt = (int)Random.Range(eventObj.m_MinMaxAffectedAmount.x, eventObj.m_MinMaxAffectedAmount.y);

                    numberText = "$" + decreaseAmt.ToString();
                    popularity.UpdatePopularityOffset(-decreaseAmt);
                }
                break;
            case NegativeEventTypes.LOSE_MONEY:
                {
                    Money money = GameStats.Instance.m_Money;
                    if (money == null)
                        return;

                    int decreaseAmt = 0;
                    if (eventObj.m_AffectedByPercentage)
                        decreaseAmt = (int)((float)money.m_CurrMoney * Random.Range(eventObj.m_MinMaxAffectedPercentages.x, eventObj.m_MinMaxAffectedPercentages.y));
                    else
                        decreaseAmt = (int)Random.Range(eventObj.m_MinMaxAffectedAmount.x, eventObj.m_MinMaxAffectedAmount.y);

                    numberText = "$" + decreaseAmt.ToString();
                    money.ReduceMoney(decreaseAmt);
                }
                break;
            case NegativeEventTypes.EVIL_COOPERATE:
                {
                    int numberToSpawn = (int)Random.Range(eventObj.m_MinMaxAffectedAmount.x, eventObj.m_MinMaxAffectedAmount.y);

                    for(int i = 0; i < numberToSpawn; ++i)
                    {
                        NPCManager.Instance.SpawnEvilPeople();
                    }

                    //TODO::
                    //get number of trees before this event
                    //get number of trees after this event
                    //THIS EVENT CAN ONLY HAPPEN WHEN THERE ARE TREES
                    //TODO:: check evil people count, if 0 event cleared
                    //put a notification on whether player manage to earn back some money from it
                    //based on lawyer
                }
                break;
        }

        UpdateUI(eventObj, numberText);
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
    LOSE_MONEY,
    EVIL_COOPERATE,
}


