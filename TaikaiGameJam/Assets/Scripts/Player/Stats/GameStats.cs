public class GameStats : SingletonBase<GameStats>
{
    public Money m_Money = new Money();
    public VolunteerStats m_VolunteerInfo = new VolunteerStats();
    public RecordingEquipment m_RecordingEquipmentStats = new RecordingEquipment();
    public LawyerServices m_LawyerServices = new LawyerServices();

    public Inventory m_Inventory = new Inventory();

    public void Awake()
    {
        m_Money.Init();
    }

    public void DeductTotalMonthFees()
    {
        m_Money.ReduceMoney(GetTotalMonthFees());
    }

    public int GetTotalMonthFees()
    {
        return m_VolunteerInfo.GetTotalMonthlyPayment() + m_RecordingEquipmentStats.GetMonthlyMaintenceFees() + m_LawyerServices.GetCurrentServiceFee();
    }
}
