public class GameStats : SingletonBase<GameStats>
{
    public Money m_Money = new Money();
    public VolunteerStats m_VolunteerInfo = new VolunteerStats();
    public RecordingEquipment m_RecordingEquipmentStats = new RecordingEquipment();
    public LawyerServices m_LawyerServices = new LawyerServices();

    public Inventory m_Inventory = new Inventory();

    //TODO:: track the number of trees there are for the shop to be sold out

    public void Awake()
    {
        m_Money.Init();
    }

    #region inventory related
    public bool CheckIsInventoryEmpty()
    {
        if (m_Inventory.m_PlantInventory == null)
            return false;

        return m_Inventory.m_PlantInventory.Count == 0;
    }

    public bool HaveInInventory(Plant_Types plantType)
    {
        return m_Inventory.HaveInInventory(plantType);
    }

    public Plant_Types GetAvilablePlantType()
    {
        return m_Inventory.GetAnyAvailablePlantType();
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
}
