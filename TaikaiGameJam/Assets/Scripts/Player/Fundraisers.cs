using UnityEngine;
using TMPro;

public class Fundraisers : MonoBehaviour
{
    [Header("Gacha percentages")]
    public Vector2 m_MinMaxSuccessPercentage = new Vector2(0.05f,0.7f);
    public Vector2 m_MinMaxSuccessIncreasePercentageAmt = new Vector2(0.3f, 0.5f);
    public Vector2 m_MinMaxFailurePercentageGetBackAmt = new Vector2(0.3f, 0.5f);

    //number of people per increase success?
    [Header("Factors affecting")]
    [Tooltip("The number of people needed to increase by 1%")]
    public int m_PopularityIncreaseRate = 50;

    [Header("UI stuff")]
    public TextMeshProUGUI[] m_NumbersTextUI;
    public int[] m_SignificantNumberPlace;

    [HideInInspector] public int m_CurrentAmtSpent = 0;

    #region UI stuff
    public void IncreaseNumber(int significantNumberPlace)
    {
        ++m_SignificantNumberPlace[significantNumberPlace];

        if (m_SignificantNumberPlace[significantNumberPlace] > 9)
            m_SignificantNumberPlace[significantNumberPlace] = 0;

        UpdateNumberUI(significantNumberPlace);
    }

    public void DecreaseNumber(int significantNumberPlace)
    {
        --m_SignificantNumberPlace[significantNumberPlace];

        if (m_SignificantNumberPlace[significantNumberPlace] < 0)
            m_SignificantNumberPlace[significantNumberPlace] = 9;

        UpdateNumberUI(significantNumberPlace);
    }

    public void UpdateNumberUI(int significantNumberPlace)
    {
        m_NumbersTextUI[significantNumberPlace].text = m_SignificantNumberPlace[significantNumberPlace].ToString();
    }
    #endregion

    //TODO:: IF PLAYER AMT IS NEGATIVE, WARN PLAYER BEFORE THEY CAN SPEND
    public void SetCurrentAmtSpent()
    {
        m_CurrentAmtSpent = 0;
        for (int i = 0; i < m_SignificantNumberPlace.Length; ++i)
        {
            m_CurrentAmtSpent += (int)(m_SignificantNumberPlace[i] * Mathf.Pow(10, i));
        }
    }

    public void CheckSuccess()
    {
        //deduct first
        GameStats.Instance.m_Money.ReduceMoney(m_CurrentAmtSpent, false);

        //get the current popularity number
        int currentPopularity = GameStats.Instance.m_Popularity.m_CurrentPopularity;
        //divide by the number per percentage increase
        float m_PercentageIncrease = (currentPopularity / m_PopularityIncreaseRate) / 100.0f;

        //add to the min successpercentage
        float m_CurrentSuccessRate = m_PercentageIncrease + m_MinMaxSuccessPercentage.x;
        m_CurrentSuccessRate = Mathf.Clamp(m_CurrentSuccessRate, m_MinMaxSuccessPercentage.x, m_MinMaxSuccessPercentage.y);

        //randomize float and check if its within the range
        if (Random.Range(0.0f,1.0f) <= m_CurrentSuccessRate) //pass
        {
            //get back the amount of money spent and more
            int moneyReturn = (int)(m_CurrentAmtSpent + m_CurrentAmtSpent * Random.Range(m_MinMaxSuccessIncreasePercentageAmt.x, m_MinMaxSuccessIncreasePercentageAmt.y));
            GameStats.Instance.m_Money.IncreaseMoney(moneyReturn);
        }
        else //failed
        {
            //lose a certain amount of money spent
            int moneyReturn = (int)(m_CurrentAmtSpent * Random.Range(m_MinMaxFailurePercentageGetBackAmt.x, m_MinMaxFailurePercentageGetBackAmt.y));
            GameStats.Instance.m_Money.IncreaseMoney(moneyReturn);
        }

        //TODO:: show UI on success or failure
        //TODO:: check bankrupt?
    }
}
