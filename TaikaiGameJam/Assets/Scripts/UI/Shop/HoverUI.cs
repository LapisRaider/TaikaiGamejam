using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject m_UIToShow;

    public void OnEnable()
    {
        if (m_UIToShow != null)
            m_UIToShow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_UIToShow != null)
            m_UIToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_UIToShow != null)
            m_UIToShow.SetActive(false);
    }
}
