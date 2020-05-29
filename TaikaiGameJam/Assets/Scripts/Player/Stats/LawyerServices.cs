using UnityEngine;

[System.Serializable]
public class LawyerServices
{
    public enum LawyerStages
    {
        NONE,
        FIRST,
        SECOND,
        THIRD,
        FORTH,
        TOTAL_STAGES //DONT BOTHER THIS
    }

    [System.Serializable]
    public class LawyerStagesData
    {
        public LawyerStages m_LawyerStage = LawyerStages.NONE;
        public int m_MontlyServiceFees = 0; //Montly service price
        public int m_ChanceIncrease = 0; //rate increase to get back money
    }

    public LawyerStagesData[] m_LawyerStagesData = new LawyerStagesData[(int)LawyerStages.TOTAL_STAGES];
    [HideInInspector] public LawyerStages m_CurrLawyerStage = LawyerStages.NONE;

    public void UpgradeLawyerService()
    {
        if (!AbleToUpgrade())
            return;

        m_CurrLawyerStage += 1;
    }

    public void DownGradeLawyerService()
    {
        if (!AbleToDownGrade())
            return;

        m_CurrLawyerStage -= 1;
    }

    public bool AbleToUpgrade()
    {
        return (m_CurrLawyerStage + 1 < LawyerStages.TOTAL_STAGES);
    }

    public bool AbleToDownGrade()
    {
        return m_CurrLawyerStage > LawyerStages.NONE;
    }

    public int GetCurrentServiceFee()
    {
        return m_LawyerStagesData[(int)m_CurrLawyerStage].m_MontlyServiceFees;
    }
}
