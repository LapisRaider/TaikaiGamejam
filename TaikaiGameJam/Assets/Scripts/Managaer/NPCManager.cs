using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : SingletonBase<NPCManager>
{
    public NPCObjectPooler m_NPCObjectPooler = new NPCObjectPooler();

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
        if (m_NPCObjectPooler != null)
            m_NPCObjectPooler.GetAndSpawnVolunteers();
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
}
