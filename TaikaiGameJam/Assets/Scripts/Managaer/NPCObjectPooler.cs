using System.Collections.Generic;
using UnityEngine;

public class NPCObjectPooler : MonoBehaviour
{
    [Header("Volunteers spawn info")]
    public GameObject m_VolunteerPrefab;
    public int m_InitialVolunteerSpawnObjPooler = 10;
    public Transform m_VolunteerParent;
    List<Volunteers> m_VolunteerList;

    [Header("Bad guys spawn info")]
    public GameObject m_BadGuysPrefab;
    public int m_InitialBadGuysSpawnObjPooler = 10;
    public Transform m_BadGuyParent;
    List<EvilPeople> m_EvilPeopleList;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnVolunteers(m_InitialVolunteerSpawnObjPooler);
        SpawnBadGuys(m_InitialBadGuysSpawnObjPooler);
    }

    #region Volunteer
    public GameObject GetAndSpawnVolunteers()
    {
        foreach (Volunteers volunteer in m_VolunteerList)
        {
            if (volunteer.gameObject.activeSelf)
                continue;

            return volunteer.gameObject;
        }

        SpawnVolunteers(m_InitialVolunteerSpawnObjPooler);

        return GetAndSpawnVolunteers();
    }

    public void SpawnVolunteers(int number)
    {
        for (int i = 0; i < number; ++i)
        {
            GameObject volunteer = Instantiate(m_VolunteerPrefab);
            volunteer.transform.parent = m_VolunteerParent;
            volunteer.SetActive(false);

            Volunteers volunteerComponent = volunteer.GetComponent<Volunteers>();
            if (volunteerComponent != null)
                m_VolunteerList.Add(volunteerComponent);
        }
    }
    #endregion

    #region BadGuy
    public GameObject GetAndSpawnBadGuy()
    {
        foreach (EvilPeople badGuy in m_EvilPeopleList)
        {
            if (badGuy.gameObject.activeSelf)
                continue;

            return badGuy.gameObject;
        }

        SpawnBadGuys(m_InitialBadGuysSpawnObjPooler);

        return GetAndSpawnBadGuy();
    }

    public void SpawnBadGuys(int number)
    {
        for (int i = 0; i < number; ++i)
        {
            GameObject badGuy = Instantiate(m_BadGuysPrefab);
            badGuy.transform.parent = m_BadGuyParent;
            badGuy.SetActive(false);

            EvilPeople badPeopleComp = badGuy.GetComponent<EvilPeople>();
            if (badPeopleComp != null)
                m_EvilPeopleList.Add(badPeopleComp);
        }
    }
    #endregion
}
