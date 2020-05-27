using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : SingletonBase<GameStats>
{
    public Money m_Money = new Money();

    public VolunteerStats m_VolunteerInfo = new VolunteerStats();

    [Header("RecordingEquipmentLvl")]
    int m_CurrLevel = 0;


}
