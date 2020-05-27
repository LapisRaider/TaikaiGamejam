using UnityEngine;

[System.Serializable]
public class Money
{
    [Header("Money")]
    public int m_BankruptcyAmt = -5000;
    public int m_CurrMoney = 100;

    public void ReduceMoney(int reduce)
    {
        m_CurrMoney -= reduce;

        if (m_CurrMoney <= m_BankruptcyAmt)
        {
            //TODO:: Lose the game
        }
    }

    public void IncreaseMoney(int increaseAmt)
    {
        m_CurrMoney += increaseAmt;
    }
}
