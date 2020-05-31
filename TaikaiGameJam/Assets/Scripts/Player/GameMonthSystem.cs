using UnityEngine;

public class GameMonthSystem : MonoBehaviour
{
    public float m_TimePerMonth = 60.0f;

    float m_CurrTimeTracker = 0.0f;
    [HideInInspector] public Months m_CurrMonth = Months.JAN;
    [HideInInspector] public int m_CurrYear = 2020;

    public void Start()
    {
        UIManager.Instance.SetMonthUI(m_CurrMonth, m_CurrYear);
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrTimeTracker += Time.deltaTime;
        if (m_CurrTimeTracker > m_TimePerMonth)
        {
            ++m_CurrMonth;

            //deduct money
            GameStats.Instance.DeductTotalMonthFees();
            m_CurrTimeTracker = 0.0f;

            //increase money for donations
            GameStats.Instance.MonthlyDonationsIncreaseMoney();
            GameStats.Instance.UpdateMonth();

            //update UI
            UIManager.Instance.SetMonthUI(m_CurrMonth, m_CurrYear);

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
    MAR,
    APR,
    MAY,
    JUN,
    JUL,
    AUG,
    SEP,
    OCT,
    NOV,
    DEC
}