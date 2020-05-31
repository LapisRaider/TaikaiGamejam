using UnityEngine;

[CreateAssetMenu(menuName = "Events")]
public class EventScriptableObj : ScriptableObject
{
    public bool m_OneTimeOnly = false;

    public PositiveEventTypes m_PositiveEventType = PositiveEventTypes.P_NONE;
    public NegativeEventTypes m_NegativeEventType = NegativeEventTypes.N_NONE;

    [Header("Numbers")]
    public bool m_AffectedByPercentage = true;
    public Vector2 m_MinMaxAffectedPercentages = new Vector2(0.0f,1.0f);
    public Vector2 m_MinMaxAffectedAmount = new Vector2(0,500);

    [Header("UI info")]
    public Sprite[] m_EventPicture;
    [TextArea(3, 5)]
    public string[] m_Description;
    public string[] m_NameOfPersonInvolve;
    public string[] m_Title;
}

