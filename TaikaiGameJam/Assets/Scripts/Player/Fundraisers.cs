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
    [HideInInspector] public int[] m_SignificantNumberPlace;
    public TextMeshProUGUI m_PercentageText;

    [Header("Warning UI")]
    public GameObject m_Warning;

    [Header("Win Lose UI")]
    public GameObject m_SuccessUI;
    public TextMeshProUGUI m_SuccessEarnings;
    public GameObject m_NoSuccessUI;
    public TextMeshProUGUI m_NoSuccessEarnings;


    [HideInInspector] public int m_CurrentAmtSpent = 0;
    [HideInInspector] public float m_ChanceOfSuccess = 0.0f;

    //TODO:: make it once a month only

    public void OnEnable()
    {
        if (m_Warning != null)
            m_Warning.SetActive(false);

        if (m_SuccessUI != null)
            m_SuccessUI.SetActive(false);

        if (m_NoSuccessUI != null)
            m_NoSuccessUI.SetActive(false);

        foreach (TextMeshProUGUI text in m_NumbersTextUI)
        {
            text.text = (0).ToString();
        }

        for (int i =0; i < m_SignificantNumberPlace.Length; ++i)
        {
            m_SignificantNumberPlace[i] = 0;
        }

        GetCurrentSuccessRate();
        
        if (m_PercentageText != null)
            m_PercentageText.text = "Chance of success based on popularity: " + (m_ChanceOfSuccess * 100.0f) + "%";
    }

    #region UI stuff
    public void IncreaseNumber(int significantNumberPlace)
    {
        if (significantNumberPlace < 0 || significantNumberPlace >= m_SignificantNumberPlace.Length)
            return;

        ++m_SignificantNumberPlace[significantNumberPlace];

        if (m_SignificantNumberPlace[significantNumberPlace] > 9)
            m_SignificantNumberPlace[significantNumberPlace] = 0;

        UpdateNumberUI(significantNumberPlace);
    }

    public void DecreaseNumber(int significantNumberPlace)
    {
        if (significantNumberPlace < 0 || significantNumberPlace >= m_SignificantNumberPlace.Length)
            return;

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

    public void ConfirmPayment()
    {
        SetCurrentAmtSpent();

        Money money = GameStats.Instance.m_Money;
        if (money == null)
            return;

        //make sure amt is more than 0
        if (m_CurrentAmtSpent == 0)
            return;

        //warn player is amt is going to be in the red
        if (money.m_CurrMoney - m_CurrentAmtSpent <= 0)
        {
            m_Warning.SetActive(true);
        }
        else
        {
            CheckSuccess();
        }
    }

    public void SetCurrentAmtSpent()
    {
        m_CurrentAmtSpent = 0;
        for (int i = 0; i < m_SignificantNumberPlace.Length; ++i)
        {
            m_CurrentAmtSpent += (int)(m_SignificantNumberPlace[i] * Mathf.Pow(10, (m_SignificantNumberPlace.Length - i - 1)));
        }
    }

    public void GetCurrentSuccessRate()
    {
        //get the current popularity number
        int currentPopularity = GameStats.Instance.m_Popularity.m_CurrentPopularity;
        //divide by the number per percentage increase
        float m_PercentageIncrease = (currentPopularity / m_PopularityIncreaseRate) / 100.0f;

        //add to the min successpercentage
        m_ChanceOfSuccess = m_PercentageIncrease + m_MinMaxSuccessPercentage.x;
        m_ChanceOfSuccess = Mathf.Clamp(m_ChanceOfSuccess, m_MinMaxSuccessPercentage.x, m_MinMaxSuccessPercentage.y);
    }

    public void CheckSuccess()
    {
        if (GameStats.Instance.m_Money == null)
            return;

        //deduct first
        GameStats.Instance.m_Money.ReduceMoney(m_CurrentAmtSpent, false);

        //randomize float and check if its within the range
        if (Random.Range(0.0f,1.0f) <= m_ChanceOfSuccess) //pass
        {
            //get back the amount of money spent and more
            int moneyReturn = (int)(m_CurrentAmtSpent + m_CurrentAmtSpent * Random.Range(m_MinMaxSuccessIncreasePercentageAmt.x, m_MinMaxSuccessIncreasePercentageAmt.y));
            GameStats.Instance.m_Money.IncreaseMoney(moneyReturn);

            if (m_SuccessEarnings != null)
                m_SuccessEarnings.text = "$" + moneyReturn.ToString() + " was donated";

            ShowSuccesspopup(true);
        }
        else //failed
        {
            //lose a certain amount of money spent
            int moneyReturn = (int)(m_CurrentAmtSpent * Random.Range(m_MinMaxFailurePercentageGetBackAmt.x, m_MinMaxFailurePercentageGetBackAmt.y));
            GameStats.Instance.m_Money.IncreaseMoney(moneyReturn);

            if (m_SuccessEarnings != null)
                m_NoSuccessEarnings.text = "You lost $" + (m_CurrentAmtSpent - moneyReturn).ToString();

            ShowSuccesspopup(false);
        }

        //TODO:: check bankrupt?
    }

    #region UI popups
    public void SetWarningUIActive(bool active)
    {
        if (m_Warning != null)
            m_Warning.SetActive(active);
    }

    public void SetSuccessUIActive(bool active)
    {
        if (m_SuccessUI != null)
            m_SuccessUI.SetActive(active);
    }

    public void SetUnsuccessfulUIActive(bool active)
    {
        if (m_NoSuccessUI != null)
            m_NoSuccessUI.SetActive(active);
    }

    public void ShowSuccesspopup(bool success)
    {
        if (success)
            SetSuccessUIActive(true);
        else
            SetUnsuccessfulUIActive(true);
    }
    #endregion
}
