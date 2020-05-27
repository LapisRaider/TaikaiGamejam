using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServicesUI : MonoBehaviour
{
    [Header("Volunteer Hiring")]
    public Button m_HireVolunteerButton;
    public Button m_FireVolunteerButton;
    public TextMeshProUGUI m_MonthlySalaryText;
    public TextMeshProUGUI m_EmployeeAmtText;

    [Header("Video Equipment")]
    public Button m_UpgradeVideoButton;
    public Button m_DowngradeVideoButton;
    public TextMeshProUGUI m_VideoMaintenceCostText;
    public TextMeshProUGUI m_VideoUpgradePriceText;

    [Header("Lawyer services")]
    public Button m_UpgradeLawyer;
    public Button m_DowngradeLawyer;
    public TextMeshProUGUI m_LawyerMaintenceCost;

    public void OnEnable()
    {
        //setup initial details
        UpdateVolunteerDetailsUI();
    }

    #region SetVolunteerHiring UI
    public void UpdateVolunteerDetailsUI()
    {
        VolunteerStats volunteerStats = GameStats.Instance.m_VolunteerInfo;

        m_EmployeeAmtText.SetText(volunteerStats.m_CurrVolunteerAmt.ToString() + " / " + volunteerStats.m_MaxVolunteerNo.ToString());
        m_MonthlySalaryText.SetText("$" + volunteerStats.m_MonthlyPayment.ToString());

        //if cannot fire make it not interactable
        m_FireVolunteerButton.interactable = volunteerStats.CanFire();
        //if cannot hire make it not interactable
        m_HireVolunteerButton.interactable = volunteerStats.CanHire();
    }

    public void FireVolunteer()
    {
        VolunteerStats volunteerStats = GameStats.Instance.m_VolunteerInfo;
        volunteerStats.FireVolunteer();

        UpdateVolunteerDetailsUI();
    }

    public void HireVolunteer()
    {
        VolunteerStats volunteerStats = GameStats.Instance.m_VolunteerInfo;
        volunteerStats.GetMoreVolunteer();

        UpdateVolunteerDetailsUI();
    }
    #endregion


    #region Video Equipment UI
    public void UpdateVideoEquipmentDetailsUI()
    {
        RecordingEquipment recordingEquipment = GameStats.Instance.m_RecordingEquipmentStats;

        //m_VideoMaintenceCostText.SetText(volunteerStats.m_CurrVolunteerAmt.ToString() + " / " + volunteerStats.m_MaxVolunteerNo.ToString());
        //m_VideoUpgradePriceText.SetText("$" + volunteerStats.m_MonthlyPayment.ToString());

        //TODO:: all the text, if cannot upgrade change text to something else 

        m_UpgradeVideoButton.interactable = recordingEquipment.AbleToUpgrade();
        m_DowngradeVideoButton.interactable = recordingEquipment.AbleToDowngrade();
    }

    public void UpgradeEquipment()
    {
        RecordingEquipment recordingEquipment = GameStats.Instance.m_RecordingEquipmentStats;
        recordingEquipment.Upgrade();

        UpdateVideoEquipmentDetailsUI();
    }

    public void DowngradeEquipment()
    {
        RecordingEquipment recordingEquipment = GameStats.Instance.m_RecordingEquipmentStats;
        recordingEquipment.DownGrade();

        UpdateVideoEquipmentDetailsUI();
    }
    #endregion

    #region Lawyer
    //TODO:: Lawyer related UI
    #endregion

}
