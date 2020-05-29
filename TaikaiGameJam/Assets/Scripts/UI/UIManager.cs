using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Money Related")]
    public TextMeshProUGUI m_CurrentMoneyText;
    public Color m_MoneyLessThanZeroTextColor = Color.red;
    public Color m_MoneyDefaultColor = Color.black;

    [Header("Popularity Related")]
    public TextMeshProUGUI m_PopularityCountText;

    //[Header("Temperature related")]
    //public TextMeshProUGUI m_

    //TODO:: Temperature UI

    public void SetCurrentMoneyUI(int currAmt)
    {
        m_CurrentMoneyText.text = currAmt.ToString();

        if (currAmt < 0)
        {
            m_CurrentMoneyText.color = m_MoneyLessThanZeroTextColor;
        }
        else
        {
            m_CurrentMoneyText.color = m_MoneyDefaultColor;
        }
    }

    public void SetPopularityUI(int currentPopularity)
    {
        m_PopularityCountText.text = currentPopularity.ToString();
    }
}
