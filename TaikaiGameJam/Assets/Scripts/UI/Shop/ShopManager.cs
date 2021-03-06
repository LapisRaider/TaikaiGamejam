﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("UI info")]
    public Button m_RightButton;
    public Button m_LeftButton;

    public List<GameObject> m_Pages;
    int m_CurrentPage = 0;

    [Header("Video Equipment")]
    public Button m_UpgradeVideoButton;
    public Button m_DowngradeVideoButton;
    public Sprite m_NoHaveVideoEquipmentSprite;
    public TextMeshProUGUI m_VideoUpgradePriceText;
    public TextMeshProUGUI[] m_VideoMaintenceCostText = new TextMeshProUGUI[(int)VideoEquipmentUIStates.ALL];
    public Image[] m_VideoStatesSprites = new Image[(int)VideoEquipmentUIStates.ALL];
    public TextMeshProUGUI m_UpgradeDescriptionText;
    public TextMeshProUGUI m_DowngradeDescriptionText;

    public enum VideoEquipmentUIStates
    {
        OLD,
        CURR,
        NEW,
        ALL
    }

    public void OnEnable()
    {
        m_CurrentPage = 0;
        UpdateUIShown();
        UpdateVideoEquipmentDetailsUI();
    }

    public void UpdateUIShown()
    {
        for (int i =0; i < m_Pages.Count; ++i)
        {
            m_Pages[i].SetActive(m_CurrentPage == i);
        }

        if (m_RightButton != null)
            m_RightButton.interactable = m_CurrentPage + 1 < m_Pages.Count;

        if (m_LeftButton != null)
            m_LeftButton.interactable = m_CurrentPage > 0;
    }

    public void GoRight()
    {
        if (m_CurrentPage + 1 >= m_Pages.Count)
            return;

        m_CurrentPage += 1;

        UpdateUIShown();
        SoundManager.Instance.Play("Click");
    }

    public void GoLeft()
    {
        if (m_CurrentPage <= 0)
            return;

        m_CurrentPage -= 1;

        UpdateUIShown();
        SoundManager.Instance.Play("Click");
    }

    #region Video Equipment UI
    public void UpdateVideoEquipmentDetailsUI()
    {
        RecordingEquipment recordingEquipment = GameStats.Instance.m_RecordingEquipmentStats;
        if (recordingEquipment == null)
            return;

        RecordingEquipment.UpgradeStages currStage = recordingEquipment.m_CurrLevel;

        if (recordingEquipment.AbleToUpgrade())
            m_VideoUpgradePriceText.text = "$" + recordingEquipment.GetNextUpgradePrice();
        else
            m_VideoUpgradePriceText.text = "MAX";

        //CHANGE THE SPRITE ACCORDINGLY
        for(int i = 0; i< m_VideoStatesSprites.Length; ++i)
        {
            m_VideoStatesSprites[i].sprite = recordingEquipment.GetSpriteMode(currStage + (i-1));

            //reach the max limit
            if (m_VideoStatesSprites[i].sprite == null)
            {
                if (m_NoHaveVideoEquipmentSprite != null)
                {
                    m_VideoStatesSprites[i].sprite = m_NoHaveVideoEquipmentSprite;
                }

                //set the maintence text properly
                if (i == (int)VideoEquipmentUIStates.OLD)
                    m_DowngradeDescriptionText.text = "Yours is the oldest";
                else if (i == (int)VideoEquipmentUIStates.NEW)
                    m_UpgradeDescriptionText.text = "Yours is the latest";

                m_VideoMaintenceCostText[i].text = "X";
            }
            else
            {
                if (i == (int)VideoEquipmentUIStates.OLD)
                    m_DowngradeDescriptionText.text = "Trade in, 20% money back, lose followers";
                else if (i == (int)VideoEquipmentUIStates.NEW)
                    m_UpgradeDescriptionText.text = "Increase followers";

                //set the maintence text to be the same
                m_VideoMaintenceCostText[i].text = "$" + recordingEquipment.GetMaintenanceFees(currStage + (i - 1)).ToString();
            }
        }

        m_UpgradeVideoButton.interactable = recordingEquipment.AbleToUpgrade();
        m_DowngradeVideoButton.interactable = recordingEquipment.AbleToDowngrade();
    }

    public void UpgradeEquipment()
    {
        RecordingEquipment recordingEquipment = GameStats.Instance.m_RecordingEquipmentStats;
        recordingEquipment.Upgrade();

        UpdateVideoEquipmentDetailsUI();
        SoundManager.Instance.Play("Click");
    }

    public void DowngradeEquipment()
    {
        RecordingEquipment recordingEquipment = GameStats.Instance.m_RecordingEquipmentStats;
        recordingEquipment.DownGrade();

        UpdateVideoEquipmentDetailsUI();
        SoundManager.Instance.Play("Click");
    }
    #endregion
}
