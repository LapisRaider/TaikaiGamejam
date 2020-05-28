using UnityEngine;

[System.Serializable]
public class RecordingEquipment
{
    public enum UpgradeStages
    {
        NOOB_LV,
        BETTER_LV,
        BEST_LV,
        TOTAL_MAXLV,
    }

    [Tooltip("Monthly maintenence")]
    public int[] m_MonthlyMaintenceFees = new int[(int)UpgradeStages.TOTAL_MAXLV];

    [Header("Popularity rate")]
    public float[] m_PopularityRate = new float[(int)UpgradeStages.TOTAL_MAXLV];

    [Header("Downgrade/Upgrade")]
    //the amount of money to give back to the player when they downgrade
    public float m_DowngradePercentage = 0.2f;
    public int[] m_UpgradePrice = new int[(int)UpgradeStages.TOTAL_MAXLV];

    [HideInInspector] public UpgradeStages m_CurrLevel = UpgradeStages.NOOB_LV;

    //TODO:: the sprites for EACH equuipment level

    public void Upgrade()
    {
        if (!AbleToUpgrade())
            return;

        m_CurrLevel += 1;

        Money money = GameStats.Instance.m_Money;
        if (money != null)
            money.ReduceMoney(m_MonthlyMaintenceFees[(int)m_CurrLevel]);

        //UPDATE POPULARITY
        GameStats.Instance.UpdatePopularityInfo();
    }

    public void DownGrade()
    {
        if (!AbleToDowngrade())
            return;

        //give back some money when they downgrade
        int moneyBack = (int)(m_MonthlyMaintenceFees[(int)m_CurrLevel] * m_DowngradePercentage);
        Money money = GameStats.Instance.m_Money;
        if (money != null)
            money.ReduceMoney(moneyBack);

        m_CurrLevel -= 1;

        //UPDATE POPULARITY
        GameStats.Instance.UpdatePopularityInfo();
    }

    public bool AbleToUpgrade()
    {
        return m_CurrLevel + 1 < UpgradeStages.TOTAL_MAXLV;
    }

    public bool AbleToDowngrade()
    {
        return m_CurrLevel > 0;
    }

    public int GetMonthlyMaintenceFees()
    {
        return m_MonthlyMaintenceFees[(int)m_CurrLevel];
    }

    public float GetPopularityRate()
    {
        return m_PopularityRate[(int)m_CurrLevel];
    }
}
