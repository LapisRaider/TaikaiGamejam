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
    public TextMeshProUGUI m_LawyerMonthlySalaryText;
    public TextMeshProUGUI m_LawyerEmployeeAmtText;


    public void OnEnable()
    {
        //setup initial details
        UpdateVolunteerDetailsUI();
        UpdateLawyerDetailsUI();
    }

    #region SetVolunteerHiring UI
    public void UpdateVolunteerDetailsUI()
    {
        VolunteerStats volunteerStats = GameStats.Instance.m_VolunteerInfo;

        m_EmployeeAmtText.SetText( "Employees: " + volunteerStats.m_CurrVolunteerAmt.ToString() + " / " + volunteerStats.m_MaxVolunteerNo.ToString());
        m_MonthlySalaryText.SetText("Salary: $" + volunteerStats.m_MonthlyPayment.ToString() + "/month");

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
    public void UpdateLawyerDetailsUI()
    {
        LawyerServices lawyer = GameStats.Instance.m_LawyerServices;

        m_LawyerEmployeeAmtText.SetText("Employees: " + lawyer.m_CurrentLawyerNumber.ToString() + " / " + lawyer.m_MaxLawyerNumber.ToString());
        m_LawyerMonthlySalaryText.SetText("Salary: $" + lawyer.m_MonthlyServiceFees.ToString() + "/month");

        //if cannot fire make it not interactable
        m_DowngradeLawyer.interactable = lawyer.AbleToDownGrade();
        //if cannot hire make it not interactable
        m_UpgradeLawyer.interactable = lawyer.AbleToUpgrade();
    }

    public void FireLawyer()
    {
        LawyerServices lawyer = GameStats.Instance.m_LawyerServices;
        lawyer.DownGradeLawyerService();

        UpdateLawyerDetailsUI();
    }

    public void HireLawyer()
    {
        LawyerServices lawyer = GameStats.Instance.m_LawyerServices;
        lawyer.UpgradeLawyerService();

        UpdateLawyerDetailsUI();
    }
    #endregion
}
