﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Money Related")]
    public TextMeshProUGUI m_CurrentMoneyText;
    public Color m_MoneyLessThanZeroTextColor = Color.red;
    public Color m_MoneyDefaultColor = Color.black;

    [Header("Popularity Related")]
    public TextMeshProUGUI m_PopularityCountText;

    [Header("Temperature related")]
    public TextMeshProUGUI m_TemperatureText;
    public string[] m_TemperatureWords = new string[(int)TemperatureType.ALL_TYPES];
    public Color[] m_TemperatureColors = new Color[(int)TemperatureType.ALL_TYPES];

    [Header("Date related")]
    public TextMeshProUGUI m_MonthText;
    public TextMeshProUGUI m_YearText;

    [Header("Pages Related")]
    public GameObject m_ParentPages;
    public GameObject[] m_Pages;

    public enum Pages
    {
        SHOP_PAGE,
        FUND_RAISER,
        SERVICES,
        CALENDER,
        TOTAL_PAGE
    }

    public void SetMonthUI(Months month, int year)
    {
        m_MonthText.text = month.ToString();
        m_YearText.text = year.ToString();
    }

    public void SetTemperatureUI(TemperatureType currTemp)
    {
        m_TemperatureText.text = m_TemperatureWords[(int)(currTemp)];
        m_TemperatureText.color = m_TemperatureColors[(int)(currTemp)];
    }

    public void SetCurrentMoneyUI(int currAmt)
    {
        bool negative = false;
        m_CurrentMoneyText.text = GetRoundedNumberText(currAmt, ref negative);

        if (negative)
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
        bool negative = false;
        m_PopularityCountText.text = GetRoundedNumberText(currentPopularity, ref negative);

        if (negative)
        {
            m_PopularityCountText.color = m_MoneyLessThanZeroTextColor;
        }
        else
        {
            m_PopularityCountText.color = m_MoneyDefaultColor;
        }
    }

    public string GetRoundedNumberText(int currAmt, ref bool symbol)
    {
        string negativeSign = "";
        string sentence = "";
        symbol = false;
        if (currAmt < 0)
        {
            negativeSign = "-";
            symbol = true;
            currAmt = Mathf.Abs(currAmt);
        }

        if (currAmt >= 1000000)
        {
            int million = currAmt / 1000000;
            sentence = negativeSign + million.ToString() + "mil";
        }
        else if (currAmt >= 1000)
        {
            int thousand = currAmt / 1000;
            int hundred = (currAmt - thousand * 1000) / 100;
            sentence = negativeSign + thousand.ToString() + "." + hundred.ToString() + "k";
        }
        else
        {
            sentence = negativeSign + currAmt.ToString();
        }

        return sentence;
    }

    #region SetUI
    public void SetPagesActive(int currentPage)
    {
        SoundManager.Instance.Play("Click");

        for (int i = 0; i < m_Pages.Length; ++i)
        {
            m_Pages[i].SetActive(currentPage == i);
        }

        m_ParentPages.SetActive(true);
    }

    public void ClosePage()
    {
        SoundManager.Instance.Play("Click");

        for (int i = 0; i < m_Pages.Length; ++i)
        {
            m_Pages[i].SetActive(false);
        }

        m_ParentPages.SetActive(false);
    }
    #endregion

    public void RestartGame()
    {
        SoundManager.Instance.Play("Click");
        SceneManager.LoadScene("MainMenu");
    }
}
