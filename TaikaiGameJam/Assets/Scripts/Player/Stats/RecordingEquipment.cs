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

    [Tooltip("the monthly maintenence for each level")]
    public int[] m_MonthlyMaintenceFees = new int[(int)UpgradeStages.TOTAL_MAXLV];
    public int[] m_UpgradePrice = new int[(int)UpgradeStages.TOTAL_MAXLV];

    //the amount of money to give back to the player when they downgrade
    public float m_DowngradePercentage = 0.2f;

    [HideInInspector] public UpgradeStages m_CurrLevel = UpgradeStages.NOOB_LV;

    //TODO:: the sprites for EACH equuipment level
    //TODO:: how it affects the popularity ranking

    public void Upgrade()
    {
        if (!AbleToUpgrade())
            return;

        m_CurrLevel += 1;

        Money money = GameStats.Instance.m_Money;
        if (money != null)
            money.ReduceMoney(m_MonthlyMaintenceFees[(int)m_CurrLevel]);
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
}
