using UnityEngine;

[System.Serializable]
public class VolunteerStats
{
    [Header("Volunteer")]
    public int m_MaxVolunteerNo = 30;
    public int m_MonthlyPayment = 100; 
    [HideInInspector] public int m_CurrVolunteerAmt = 0;

    public void GetMoreVolunteer()
    {
        //reach the volunteer limit
        if (!CanHire())
            return;

        m_CurrVolunteerAmt += 1;
        NPCManager.Instance.SpawnVolunteer();
    }

    public void FireVolunteer()
    {
        if (!CanFire())
            return;

        m_CurrVolunteerAmt -= 1;
        NPCManager.Instance.FireVolunteer();
    }

    public bool CanFire()
    {
        return m_CurrVolunteerAmt > 0;
    }

    public bool CanHire()
    {
        return (m_CurrVolunteerAmt + 1 <= m_MaxVolunteerNo);
    }

    public int GetTotalMonthlyPayment()
    {
        return m_MonthlyPayment * m_CurrVolunteerAmt;
    }
}
