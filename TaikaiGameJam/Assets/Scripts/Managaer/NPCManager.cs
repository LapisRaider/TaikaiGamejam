﻿using UnityEngine;

public class NPCManager : SingletonBase<NPCManager>
{
    public NPCObjectPooler m_NPCObjectPooler = new NPCObjectPooler();
    public Volunteers m_PlayerVolunteer;

    // Start is called before the first frame update
    void Awake()
    {
        m_NPCObjectPooler.Init();
    }

    #region sendingMessages
    //warn the nearest volunteer there is someone cutting down the trees
    public Volunteers WarnNearestVolunteer(EvilPeople evilPerson)
    {
        float closestDist = Mathf.Infinity;
        Volunteers nearestVolunteer = null;

        if (m_PlayerVolunteer != null)
        {
            if (m_PlayerVolunteer.CheckCanChase())
            {
                float newDist = Vector2.SqrMagnitude(evilPerson.transform.position - m_PlayerVolunteer.transform.position);
                nearestVolunteer = m_PlayerVolunteer;
            }
        }

        foreach (Volunteers volunteer in m_NPCObjectPooler.m_VolunteerList)
        {
            if (!volunteer.gameObject.activeSelf)
                continue;

            //make sure volunteer is available to start chasing
            if (!volunteer.CheckCanChase())
                continue;

            float newDist = Vector2.SqrMagnitude(evilPerson.transform.position - volunteer.transform.position);
            if (newDist < closestDist)
            {
                closestDist = newDist;
                nearestVolunteer = volunteer;
            }
        }

        if (nearestVolunteer != null)
        {
            //pass the evil person to the nearest volunteer
            nearestVolunteer.ChaseEvilPerson(evilPerson);
        }

        return nearestVolunteer;
    }
    #endregion

    #region volunteer related
    public void SpawnVolunteer()
    {
        GameObject volunteer = null;

        if (m_NPCObjectPooler != null)
            volunteer = m_NPCObjectPooler.GetAndSpawnVolunteers();

        //SET THE NPCS AT THE CORRECT POSITION
        if (volunteer != null)
        {
            volunteer.transform.localPosition = Vector2.zero;
            volunteer.SetActive(true);
        }
    }

    public void FireVolunteer()
    {
        //get one volunteer and set inactive
        if (m_NPCObjectPooler != null)
        {
            foreach(Volunteers volunteer in m_NPCObjectPooler.m_VolunteerList)
            {
                if (volunteer.gameObject.activeSelf)
                {
                    volunteer.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
    #endregion

    #region evil people related
    public void SpawnEvilPeople()
    {
        if (m_NPCObjectPooler != null)
        {
            GameObject evilPeople = m_NPCObjectPooler.GetAndSpawnBadGuy();
            evilPeople.SetActive(true);
        }
    }

    public int CheckActiveEvilPeople()
    {
        return m_NPCObjectPooler.GetNumberOfActiveBadGuys();
    }
    #endregion
}
