using System.Collections.Generic;
using UnityEngine;

public class EvilPeople : MonoBehaviour
{
    [Header("Info")]
    SpriteRenderer m_SpriteRenderer;

    [Header("Movement")]
    public Vector2 m_MinMaxSpeed = new Vector2(0.1f, 1.0f);

    [Header("Move to tree State")]
    [Tooltip("The min number of grid before it accepts the tree")]
    public int m_MinNoOfGrid = 6;
    public float m_CutTreeDist = 1.5f;
    PlantTree m_NearestTree = null;
    Vector2 m_Destination = Vector2.zero;

    [Header("Destroy Tree")]
    public int m_HealthDeduction = 2;
    public float m_DestructionRate = 0.5f;
    float m_DestructionTimer = 0.0f;

    [Header("Chase state")]
    public float m_RunSpeed = 3.0f;
    public float m_MinSafeDist = 4.0f;
    Transform m_NearestVolunteerChasing = null;

    public enum States
    {
        MOVE_TO_TREE,
        CUTTING_TREE,
        RUN
    }

    CharacterController m_CharacterController;
    Animator m_Animator;

    Vector2 m_Dir;
    float m_Speed = 1.0f;

    States m_CurrentState = States.MOVE_TO_TREE;

    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_Speed = Random.Range(m_MinMaxSpeed.x, m_MinMaxSpeed.y);
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        //spawn at a random location outside the grid
        BoundsInt mapBoundary = MapManager.Instance.m_MapBoundaryGridNo;

        int sideToSpawn = Random.Range(0,4);
        Vector2Int spawnGridPos = Vector2Int.zero;
        switch(sideToSpawn)
        {
            case 0: //up
                spawnGridPos.y = mapBoundary.yMax + 1;
                spawnGridPos.x = Random.Range(mapBoundary.xMin, mapBoundary.xMax);
                break;
            case 1: //down
                spawnGridPos.y = mapBoundary.yMin - 1;
                spawnGridPos.x = Random.Range(mapBoundary.xMin, mapBoundary.xMax);
                break;
            case 2: //left
                spawnGridPos.x = mapBoundary.xMin - 1;
                spawnGridPos.y = Random.Range(mapBoundary.yMin, mapBoundary.yMax);
                break;
            case 3: //right
                spawnGridPos.x = mapBoundary.xMax + 1;
                spawnGridPos.y = Random.Range(mapBoundary.yMin, mapBoundary.yMax);
                break;
        }

        transform.position = MapManager.Instance.GetGridPosToWorld(spawnGridPos);

        EnterFindTreeState();
    }

    public void Update()
    {
        switch (m_CurrentState)
        {
            case States.MOVE_TO_TREE:
                UpdateFindTreeState();
                break;
            case States.CUTTING_TREE:
                UpdateCutTreeState();
                break;
            case States.RUN:
                UpdateRunAwayState();
                break;
        }

        m_SpriteRenderer.sortingOrder = (int)(transform.position.y * -100);
    }

    public void ChangeState(States newState)
    {
        //check current state and exit
        switch (m_CurrentState)
        {
            case States.MOVE_TO_TREE:
                break;
            case States.CUTTING_TREE:
                break;
            case States.RUN:
                break;
        }

        //check new state and enter
        switch (newState)
        {
            case States.MOVE_TO_TREE:
                EnterFindTreeState();
                break;
            case States.CUTTING_TREE:
                EnterCutTreeState();
                break;
            case States.RUN:
                break;
        }

        m_CurrentState = newState;
    }

    #region Find Tree State
    public void EnterFindTreeState()
    {
        //TODO:: check if prev tree is still avilable

        Vector2Int m_CurrentGridPos = MapManager.Instance.GetWorldToGridPos(transform.position);

        //the nearest tree
        float closestDist = Mathf.Infinity;
        Vector2Int treeDestination = Vector2Int.zero;
        foreach (KeyValuePair<Vector2Int, PlantTree> tree in MapManager.Instance.m_TreeOnMap)
        {
            float dist = Vector2.SqrMagnitude(tree.Key - m_CurrentGridPos);
            if (dist < closestDist)
            {
                closestDist = dist;
                m_NearestTree = tree.Value;
                treeDestination = tree.Key;

                //if the dist is already in a certain radius, just stop
                if (dist < m_MinNoOfGrid * m_MinNoOfGrid)
                {
                    break;
                }
            }
        }

        m_Destination = MapManager.Instance.GetGridPosToWorld(treeDestination);
        m_Dir = (m_Destination - (Vector2)transform.position);
        m_Dir.Normalize();

        //TODO:: Check if there are any trees, if no trees, exit out of the map
    }

    public void UpdateFindTreeState()
    {
        //TODO:: change speed
        transform.position += (Vector3)(m_Dir * 5.0f * Time.deltaTime);

        Vector2 direction = m_Destination - (Vector2)transform.position;
        if (Vector2.SqrMagnitude(direction) <= m_CutTreeDist * m_CutTreeDist)
        {
            ChangeState(States.CUTTING_TREE);
            return;
        }

        if (Vector2.Dot(direction.normalized, m_Dir) < 0)
        {
            ChangeState(States.CUTTING_TREE);
            return;
        }
    }

    public void ExitFindTreeState()
    {

    }
    #endregion


    #region Cut Tree State
    public void EnterCutTreeState()
    {
        m_DestructionTimer = 0.0f;

        NPCManager.Instance.WarnNearestVolunteer(this);
    }

    public void UpdateCutTreeState()
    {
        m_DestructionTimer += Time.deltaTime;
        if (m_DestructionTimer > m_DestructionRate)
        {
            if (m_NearestTree != null)
            {
                m_NearestTree.RemoveHealth(m_HealthDeduction);
            }
        }

        //if tree doesnt exist or tree is dead, go find another tree to cut
        if (m_NearestTree == null)
        {
            ChangeState(States.MOVE_TO_TREE);
        }
        else
        {
            if (!m_NearestTree.gameObject.activeSelf)
            {
                ChangeState(States.MOVE_TO_TREE);
            }
        }

        //TODO:: if they are being chased by someone and they are close
        //ChangeState(States.MOVE_TO_TREE);
    }

    public void ExitCutTreeState()
    {
        //if tree is still alive, reset the tree health
    }
    #endregion

    #region RunAway
    public void UpdateRunAwayState()
    {
        m_Dir = transform.position - m_NearestVolunteerChasing.position;
        transform.position += (Vector3)m_Dir.normalized * m_RunSpeed * Time.deltaTime;

        //TODO:: if out of boundary, set inactive



        //if volunteer is too far, just ignore and go back to other mode
        if (Vector2.SqrMagnitude(m_Dir) > m_MinSafeDist * m_MinSafeDist)
        {
            ChangeState(States.MOVE_TO_TREE);
        }
    }

    public float test = 0.0f;

    #endregion

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Volunteer")
        {
            m_NearestVolunteerChasing = collision.transform;
            ChangeState(States.RUN);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(transform.position, m_CutTreeDist);
        Gizmos.DrawWireSphere(transform.position, m_MinSafeDist);

        Gizmos.DrawWireSphere(transform.position, test);

    }
#endif
}
