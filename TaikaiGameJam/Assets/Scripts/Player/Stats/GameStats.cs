using UnityEngine;

public class GameStats : SingletonBase<GameStats>
{
    public Money m_Money = new Money();
    public VolunteerStats m_VolunteerInfo = new VolunteerStats();
    public RecordingEquipment m_RecordingEquipmentStats = new RecordingEquipment();
    public LawyerServices m_LawyerServices = new LawyerServices();

    public Inventory m_Inventory = new Inventory();

    public Popularity m_Popularity = new Popularity();
    public Donations m_Donations = new Donations();

    public Temperature m_Temperature = new Temperature();

    [Header("Plant")]
    public int m_CurrentTreeNumber = 0;
    public int m_CurrentPlantNumber = 0;

    //TODO:: track the number of trees there are for the shop to be sold out

    public void Awake()
    {
        if (m_Money != null)
            m_Money.Init();

        if (m_Popularity != null)
            m_Popularity.Init();

        if (m_Temperature != null)
            m_Temperature.Init();
    }

    private void Update()
    {
        if (m_Popularity != null)
            m_Popularity.Update();
    }

    #region inventory related
    public bool CheckIsInventoryEmpty()
    {
        if (m_Inventory == null)
            return false;

        if (m_Inventory.m_PlantInventory == null)
            return false;

        return m_Inventory.m_PlantInventory.Count == 0;
    }

    public bool HaveInInventory(Plant_Types plantType)
    {
        if (m_Inventory == null)
            return false;

        return m_Inventory.HaveInInventory(plantType);
    }

    public Plant_Types GetAvilablePlantType()
    {
        return m_Inventory.GetAnyAvailablePlantType();
    }
    #endregion

    #region plant related
    public void UpdateCurrentPlantNumber(bool isTree, int newCount)
    {
        if (isTree)
            m_CurrentTreeNumber = newCount;
        else
            m_CurrentPlantNumber = newCount;

        UpdatePopularityInfo();
        UpdateTemperatureInfo();
    }
    #endregion

    #region popularity
    public void UpdatePopularityInfo()
    {
        if (m_Popularity == null)
            return;

        m_Popularity.UpdatePopularity();
    }
    #endregion

    #region temperature
    public void UpdateTemperatureInfo()
    {
        if (m_Temperature == null)
            return;

        m_Temperature.UpdateTemperature(m_CurrentTreeNumber);
    }
    #endregion

    public void DeductTotalMonthFees()
    {
        m_Money.ReduceMoney(GetTotalMonthFees());
    }

    public int GetTotalMonthFees()
    {
        return m_VolunteerInfo.GetTotalMonthlyPayment() + m_RecordingEquipmentStats.GetMonthlyMaintenceFees() + m_LawyerServices.GetCurrentServiceFee();
    }

    public void MonthlyDonationsIncreaseMoney()
    {
        m_Money.IncreaseMoney(GetTotalMonthDonations());
    }

    //get monthly donations fees
    public int GetTotalMonthDonations()
    {
        if (m_Donations == null)
            return 0;

        if (m_Popularity == null)
            return 0;

        return m_Donations.GetMonthlyDonation(m_CurrentTreeNumber, m_CurrentPlantNumber, m_Popularity.m_CurrentPopularity);
    }
}
