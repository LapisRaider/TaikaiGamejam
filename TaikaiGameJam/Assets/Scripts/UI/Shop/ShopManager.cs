using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI info")]
    public Button m_RightButton;
    public Button m_LeftButton;

    public List<GameObject> m_Pages;
    int m_CurrentPage = 0;

    public void OnEnable()
    {
        m_CurrentPage = 0;
        UpdateUIShown();
    }

    public void UpdateUIShown()
    {
        for (int i =0; i < m_Pages.Count; ++i)
        {
            m_Pages[i].SetActive(m_CurrentPage == i);
        }

        m_RightButton.interactable = m_CurrentPage + 1 < m_Pages.Count;
        m_LeftButton.interactable = m_CurrentPage > 0;
    }

    public void GoRight()
    {
        if (m_CurrentPage + 1 >= m_Pages.Count)
            return;

        m_CurrentPage += 1;

        UpdateUIShown();
    }

    public void GoLeft()
    {
        if (m_CurrentPage <= 0)
            return;

        m_CurrentPage -= 1;

        UpdateUIShown();
    }
}
