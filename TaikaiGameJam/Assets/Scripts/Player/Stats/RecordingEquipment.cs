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

    [HideInInspector] public UpgradeStages m_CurrLevel = UpgradeStages.NOOB_LV;

    //TODO:: the sprites for EACH equuipment level
    //TODO:: how it affects the popularity ranking

    public void Upgrade()
    {
        if (m_CurrLevel + 1 >= UpgradeStages.TOTAL_MAXLV)
            return;

        m_CurrLevel += 1;
        //TODO:: reduce the money
    }

    public void DownGrade()
    {
        if (m_CurrLevel <= 0)
            return;

        m_CurrLevel -= 1;
    }

    public int GetMonthlyMaintenceFees()
    {
        return m_MonthlyMaintenceFees[(int)m_CurrLevel];
    }
}
