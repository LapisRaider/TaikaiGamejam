using UnityEngine;

[System.Serializable]
public class LawyerServices
{
    public int m_MonthlyServiceFees = 0;
    public float m_ChanceIncreasePerLawyer = 0.1f;
    public int m_MaxLawyerNumber = 0;

    [Header("Monthly services")]
    public float m_MonthlySueChances = 0.6f; //chance per month for suing
    public Vector2 m_MinMaxPercentagePerBadEvent = new Vector2(0.05f, 0.08f); //the min and max percentage of the player's money per bad event


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
    public bool MonthlySueService()
    {
        //get number of person first
        //TODO:: UI to show u dont have lawyers
        if (m_CurrentLawyerNumber <= 0)
            return false;

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
            return true;
        }

        return false;
    }
    #endregion
}
