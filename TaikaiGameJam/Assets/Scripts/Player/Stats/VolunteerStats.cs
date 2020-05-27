using UnityEngine;

[System.Serializable]
public class VolunteerStats
{
    [Header("Volunteer")]
    public int m_MaxVolunteerNo = 30;
    public int m_MonthlyPayment = 100; 
    int m_CurrVolunteerAmt = 0;

    public void GetMoreVolunteer()
    {
        //reach the volunteer limit
        if (m_CurrVolunteerAmt > m_MaxVolunteerNo)
            return;

        m_CurrVolunteerAmt += 1;
        NPCManager.Instance.SpawnVolunteer();
    }

    public void FireVolunteer()
    {
        if (m_CurrVolunteerAmt <= 0)
            return;

        m_CurrVolunteerAmt -= 1;
        NPCManager.Instance.FireVolunteer();
    }

    public int GetTotalMonthlyPayment()
    {
        return m_MonthlyPayment * m_CurrVolunteerAmt;
    }
}
