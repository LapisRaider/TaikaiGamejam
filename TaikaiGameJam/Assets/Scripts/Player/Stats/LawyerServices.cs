using UnityEngine;
using TMPro;

[System.Serializable]
public class LawyerServices
{
    public int m_MonthlyServiceFees = 0;
    public float m_ChanceIncreasePerLawyer = 0.1f;
    public int m_MaxLawyerNumber = 0;

    [Header("Monthly services")]
    public float m_MonthlySueChances = 0.6f; //chance per month for suing
    public Vector2 m_MinMaxPercentagePerBadEvent = new Vector2(0.05f, 0.08f); //the min and max percentage of the player's money per bad event

    [Header("UI")]
    public GameObject m_UIParent;
    public TextMeshProUGUI m_DescriptionText;
    public TextMeshProUGUI m_AffectedMoneyText;

    //player lose the suing, player successfully sue, player no lawyers cant sue
    public enum LawyerServiceTypes
    {
        NO_LAWYERS,
        SUE_SUCCESSFUL,
        SUE_UNSUCCESSFUL,
        NONE
    }

    [HideInInspector] public int m_CurrentLawyerNumber = 0;

    public void UpgradeLawyerService()
    {
        if (!AbleToUpgrade())
            return;

        ++m_CurrentLawyerNumber;
    }

    public void DownGradeLawyerService()
    {
        if (!AbleToDownGrade())
            return;

        --m_CurrentLawyerNumber;
    }

    public bool AbleToUpgrade()
    {
        return (m_CurrentLawyerNumber < m_MaxLawyerNumber);
    }

    public bool AbleToDownGrade()
    {
        return m_CurrentLawyerNumber > 0;
    }

    public int GetCurrentTotalServiceFee()
    {
        return m_CurrentLawyerNumber * m_MonthlyServiceFees;
    }

    #region services
    public void UpdateSueServiceUI(LawyerServiceTypes serviceType, int numberOfBadMonths = 0, int amtOfMoney = 0)
    {
        if (m_UIParent != null)
            m_UIParent.SetActive(true);

        string text = "You have a total of " + numberOfBadMonths.ToString() + " bad events this month. ";
        switch (serviceType)
        {
            case LawyerServiceTypes.NO_LAWYERS:
                text += "You have no lawyers, unable to claim any money from your losses.";
                if (m_AffectedMoneyText != null)
                    m_AffectedMoneyText.gameObject.SetActive(false);
                break;
            case LawyerServiceTypes.SUE_SUCCESSFUL:
                text += "Luckily for you, your lawyers manage to claim some damages fund.";
                if (m_AffectedMoneyText != null)
                    m_AffectedMoneyText.gameObject.SetActive(true);
                break;
            case LawyerServiceTypes.SUE_UNSUCCESSFUL:
                text += "Your lawyers were unsuccessful in claiming damage funds this month.";
                if (m_AffectedMoneyText != null)
                    m_AffectedMoneyText.gameObject.SetActive(false);
                break;
        }

        m_DescriptionText.text = text;
        m_AffectedMoneyText.text = "You got back $" + amtOfMoney.ToString();
    }

    public bool MonthlySueService()
    {
        //check if should do monthly sue
        if (Random.Range(0.0f, 1.0f) > m_MonthlySueChances)
            return false;

        //get number of person first
        if (m_CurrentLawyerNumber <= 0)
        {
            UpdateSueServiceUI(LawyerServiceTypes.NO_LAWYERS);
            return false;
        }

        //check the number of bad events
        int badEventNumber = RandomEventManager.Instance.m_NumberOfBadEventsInAMonth;
        if (badEventNumber <= 0)
            return false;

        //check the amount chance from the total amt of lawyers we got
        float currSuccessChance = m_CurrentLawyerNumber * m_ChanceIncreasePerLawyer;
        if (Random.Range(0.0f, 1.0f) <= currSuccessChance)  //successful
        {
            //get the amt of money player can get back
            float percentageMoneyEarn = 0.0f;
            for (int i =0; i < badEventNumber; ++i)
            {
                percentageMoneyEarn += Random.Range(m_MinMaxPercentagePerBadEvent.x, m_MinMaxPercentagePerBadEvent.y);
            }

            Money money = GameStats.Instance.m_Money;
            if (money == null)
                return false;

            int moneyEarn = (int)((float)money.m_CurrMoney * percentageMoneyEarn);
            money.IncreaseMoney(moneyEarn);

            RandomEventManager.Instance.ResetMonth();

            UpdateSueServiceUI(LawyerServiceTypes.SUE_SUCCESSFUL, badEventNumber, moneyEarn);
            return true;
        }

        UpdateSueServiceUI(LawyerServiceTypes.SUE_UNSUCCESSFUL, badEventNumber);

        return false;
    }
    #endregion
}
