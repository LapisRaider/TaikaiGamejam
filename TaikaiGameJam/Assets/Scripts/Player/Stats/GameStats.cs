public class GameStats : SingletonBase<GameStats>
{
    public Money m_Money = new Money();
    public VolunteerStats m_VolunteerInfo = new VolunteerStats();
    public RecordingEquipment m_RecordingEquipmentStats = new RecordingEquipment();

    public void DeductTotalMonthFees()
    {
        m_Money.ReduceMoney(GetTotalMonthFees());
    }

    public int GetTotalMonthFees()
    {
        return m_VolunteerInfo.GetTotalMonthlyPayment() + m_RecordingEquipmentStats.GetMonthlyMaintenceFees();
    }
}
