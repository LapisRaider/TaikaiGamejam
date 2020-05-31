using UnityEngine;

public class PlantTree : Plant
{
    public ParticleSystem m_ParticleSystem;
    public AudioSource m_AudioSource;

    private void OnDisable()
    {
        if (m_AudioSource != null)
        {
            m_AudioSource.Stop();
        }
    }

    public override void Dead()
    {
        base.Dead();

        //remove itself from the mapmanager
        MapManager.Instance.RemoveTree(m_PlantGridPos);
    }

    public override void RemoveHealth(int healthDeduct)
    {
        base.RemoveHealth(healthDeduct);

        if (m_ParticleSystem != null)
            m_ParticleSystem.Play();

        if (m_AudioSource != null)
        {
            if (!m_AudioSource.isPlaying)
            {
                SoundManager.Instance.Play("WoodCut", m_AudioSource);

                if (m_Animator != null)
                {
                    m_Animator.SetTrigger("Shaking");
                }
            }
        }
    }
}
