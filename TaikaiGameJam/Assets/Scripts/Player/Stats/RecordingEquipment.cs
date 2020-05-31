using UnityEngine;

[System.Serializable]
public class RecordingEquipment
{
    public enum UpgradeStages
    {
        NONE,
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

    [HideInInspector] public UpgradeStages m_CurrLevel = UpgradeStages.NONE;

    //the sprites for EACH equuipment level
    [Header("UI stuff")]
    public Sprite[] m_EquipmentSprite = new Sprite[(int)UpgradeStages.TOTAL_MAXLV];

    public void Upgrade()
    {
        if (!AbleToUpgrade())
            return;

        m_CurrLevel += 1;

        Money money = GameStats.Instance.m_Money;
        if (money != null)
            money.ReduceMoney(m_UpgradePrice[(int)m_CurrLevel]);

        //UPDATE POPULARITY
        GameStats.Instance.UpdatePopularityInfo();
    }

    public void DownGrade()
    {
        if (!AbleToDowngrade())
            return;

        //give back some money when they downgrade
        int moneyBack = (int)(m_UpgradePrice[(int)m_CurrLevel] * m_DowngradePercentage);
        Money money = GameStats.Instance.m_Money;
        if (money != null)
            money.IncreaseMoney(moneyBack);

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

    public int GetMaintenanceFees(UpgradeStages upgradeStages)
    {
        if (upgradeStages >= UpgradeStages.TOTAL_MAXLV || upgradeStages < 0)
            return 0;

        return m_MonthlyMaintenceFees[(int)upgradeStages];
    }

    public float GetPopularityRate()
    {
        return m_PopularityRate[(int)m_CurrLevel] + 1.0f;
    }

    public int GetNextUpgradePrice()
    {
        if (AbleToUpgrade())
            return m_UpgradePrice[(int)m_CurrLevel + 1];
        else
            return 0;
    }

    public Sprite GetSpriteMode(UpgradeStages upgradeStages)
    {
        if (upgradeStages >= UpgradeStages.TOTAL_MAXLV || upgradeStages < 0)
            return null;

        return m_EquipmentSprite[(int)upgradeStages];
    }
}
