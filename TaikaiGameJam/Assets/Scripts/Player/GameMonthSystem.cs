using UnityEngine;

public class GameMonthSystem : MonoBehaviour
{
    public float m_TimePerMonth = 60.0f;

    float m_CurrTimeTracker = 0.0f;
    [HideInInspector] public Months m_CurrMonth = Months.JAN;
    [HideInInspector] public int m_CurrYear = 2020;

    // Update is called once per frame
    void Update()
    {
        m_CurrTimeTracker += Time.deltaTime;
        if (m_CurrTimeTracker > m_TimePerMonth)
        {
            ++m_CurrMonth;

            //deduct money
            GameStats.Instance.DeductTotalMonthFees();

            //increase money for donations
            GameStats.Instance.MonthlyDonationsIncreaseMoney();

            if (m_CurrMonth > Months.DEC)
            {
                m_CurrMonth = Months.JAN;
                ++m_CurrYear;
            }
        }
    }
}


public enum Months
{
    JAN = 1,
    FEB,
    MARCH,
    APR,
    MAY,
    JUNE,
    JUL,
    AUG,
    SEPT,
    OCT,
    NOV,
    DEC
}