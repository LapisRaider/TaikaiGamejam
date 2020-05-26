public class PlantTree : Plant
{
    public override void Dead()
    {
        base.Dead();

        //remove itself from the mapmanager
        MapManager.Instance.RemoveTree(m_PlantGridPos);
    }
}
