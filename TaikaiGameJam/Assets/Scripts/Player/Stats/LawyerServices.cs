using UnityEngine;

[System.Serializable]
public class LawyerServices
{
    public int m_MonthlyServiceFees = 0;
    public float m_ChanceIncreasePerLawyer = 0.1f;
    public int m_MaxLawyerNumber = 0;

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

    public int GetCurrentServiceFee()
    {
        return m_CurrentLawyerNumber * m_MonthlyServiceFees;
    }
}
