using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCObjectPooler
{
    [Header("Volunteers spawn info")]
    public GameObject m_VolunteerPrefab;
    public int m_InitialVolunteerSpawnObjPooler = 10;
    public Transform m_VolunteerParent;
    [HideInInspector] public List<Volunteers> m_VolunteerList = new List<Volunteers>();

    [Header("Bad guys spawn info")]
    public GameObject m_BadGuysPrefab;
    public int m_InitialBadGuysSpawnObjPooler = 10;
    public Transform m_BadGuyParent;
    [HideInInspector] List<EvilPeople> m_EvilPeopleList = new List<EvilPeople>();

    // Start is called before the first frame update
    public void Init()
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
            GameObject volunteer = GameObject.Instantiate(m_VolunteerPrefab);
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
            GameObject badGuy = GameObject.Instantiate(m_BadGuysPrefab);
            badGuy.transform.parent = m_BadGuyParent;
            badGuy.SetActive(false);

            EvilPeople badPeopleComp = badGuy.GetComponent<EvilPeople>();
            if (badPeopleComp != null)
                m_EvilPeopleList.Add(badPeopleComp);
        }
    }

    public int GetNumberOfActiveBadGuys()
    {
        int numberOfActiveBadGuys = 0;
        foreach (EvilPeople badGuy in m_EvilPeopleList)
        {
            if (badGuy.gameObject.activeSelf)
                ++numberOfActiveBadGuys;
        }

        return numberOfActiveBadGuys;
    }
    #endregion
}
