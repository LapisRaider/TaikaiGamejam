using UnityEngine;

[System.Serializable]
public class Money
{
    [Header("Money")]
    public int m_BankruptcyAmt = -5000;
    public int m_CurrMoney = 100;

    public GameObject m_LoseUI;

    public void Init()
    {
        UIManager.Instance.SetCurrentMoneyUI(m_CurrMoney);
    }

    public void ReduceMoney(int reduce, bool checkBankrupt = true)
    {
        m_CurrMoney -= reduce;
        UIManager.Instance.SetCurrentMoneyUI(m_CurrMoney);

        if (checkBankrupt)
        {
            if (m_CurrMoney <= m_BankruptcyAmt)
            {
                if (m_LoseUI != null)
                    m_LoseUI.SetActive(true);
            }
        }
    }

    public void IncreaseMoney(int increaseAmt)
    {
        m_CurrMoney += increaseAmt;

        UIManager.Instance.SetCurrentMoneyUI(m_CurrMoney);
    }
}
