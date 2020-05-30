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

    #region Lawyer
    //TODO:: Lawyer related UI
    #endregion

}
