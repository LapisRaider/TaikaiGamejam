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
            if (!volunteer.enabled)
                continue;

            //TODO:: Make sure volunteer is not already chasing someone else

            float newDist = Vector2.SqrMagnitude(evilPerson.transform.position - volunteer.transform.position);
            if (newDist < closestDist)
            {
                closestDist = newDist;
                nearestVolunteer = volunteer;
            }
        }

        if (nearestVolunteer != null)
        {
            //TODO:: pass the evil person to the nearest volunteer
            //volunteer change state to chase
        }

        return nearestVolunteer;
    }

    #endregion
}
