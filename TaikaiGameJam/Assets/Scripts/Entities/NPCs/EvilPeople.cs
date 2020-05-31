using System.Collections.Generic;
using UnityEngine;

public class EvilPeople : MonoBehaviour
{
    [Header("Info")]
    public SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer m_ItemSpriteRenderer;
    public Animator m_Animator;

    [Header("Movement")]
    public Vector2 m_MinMaxSpeed = new Vector2(0.1f, 1.0f);
    public float m_OutOfBoundsOffset = 0.5f;

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
        RUN,
        GET_OUT_OF_MAP,
    }

    Vector2 m_Dir;
    float m_Speed = 1.0f;

    States m_CurrentState = States.MOVE_TO_TREE;

    public void Awake()
    {
        m_Speed = Random.Range(m_MinMaxSpeed.x, m_MinMaxSpeed.y);
        m_CurrentState = States.MOVE_TO_TREE;
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        //spawn at a random location outside the grid
        Vector2Int spawnGridPos = GetALocationOutSideMap();
        transform.position = MapManager.Instance.GetGridPosToWorld(spawnGridPos);

        m_CurrentState = States.MOVE_TO_TREE;
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
            case States.GET_OUT_OF_MAP:
                UpdateGetOutOfMap();
                break;
        }

        if (m_Animator != null)
            m_Animator.SetFloat("HorizontalSpeed", m_Dir.x);

        if (m_ItemSpriteRenderer != null)
            m_ItemSpriteRenderer.sortingOrder = (int)(transform.position.y * -100);

        if (m_SpriteRenderer != null)
            m_SpriteRenderer.sortingOrder = (int)(transform.position.y * -100);
    }

    public void ChangeState(States newState)
    {
        States prevState = m_CurrentState;
        m_CurrentState = newState;

        //check current state and exit
        switch (prevState)
        {
            case States.MOVE_TO_TREE:
                break;
            case States.CUTTING_TREE:
                break;
            case States.RUN:
                break;
            case States.GET_OUT_OF_MAP:
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
                EnterRunAwayState();
                break;
            case States.GET_OUT_OF_MAP:
                EnterGetOutOfMap();
                break;
        }
    }

    #region Find Tree State
    public void EnterFindTreeState()
    {
        //Check if no trees, exit out of the map
        if (MapManager.Instance.m_TreeOnMap.Count <= 0)
        {
            ChangeState(States.GET_OUT_OF_MAP);
            return;
        }

        if (m_Animator != null)
        {
            m_Animator.SetTrigger("Walking");
            m_Animator.ResetTrigger("Chased");
        }

        //check if prev tree is still avilable
        if (m_NearestTree != null)
        {
            if (!m_NearestTree.m_Dead)
            {
                m_Destination = MapManager.Instance.GetGridPosToWorld(m_NearestTree.m_PlantGridPos);
                m_Dir = (m_Destination - (Vector2)transform.position);
                m_Dir.Normalize();
                return;
            }
        }

        Vector2Int m_CurrentGridPos = MapManager.Instance.GetWorldToGridPos(transform.position);

        //the nearest tree
        float closestDist = Mathf.Infinity;
        Vector2Int treeDestination = Vector2Int.zero;
        foreach (KeyValuePair<Vector2Int, PlantTree> tree in MapManager.Instance.m_TreeOnMap)
        {
            float dist = Vector2.SqrMagnitude(tree.Key - m_CurrentGridPos);
            if (dist < closestDist)
            {
                if (tree.Value.m_Dead)
                    continue;

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
    }

    public void UpdateFindTreeState()
    {
        transform.position += (Vector3)(m_Dir * m_Speed * Time.deltaTime);

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

        if (m_NearestTree == null)
        {
            EnterFindTreeState();
            return;
        }
        else
        {
            if (!m_NearestTree.gameObject.activeSelf)
            {
                EnterFindTreeState();
                return;
            }
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

        if (m_Animator != null)
        {
            m_Animator.ResetTrigger("Walking");
            m_Animator.ResetTrigger("Chased");
            m_Animator.SetTrigger("Cutting");
        }

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
                m_DestructionTimer = 0.0f;
            }
        }

        //if tree doesnt exist or tree is dead, go find another tree to cut
        if (m_NearestTree == null)
        {
            ChangeState(States.MOVE_TO_TREE);
            return;
        }
        else
        {
            if (m_NearestTree.m_Dead)
            {
                ChangeState(States.MOVE_TO_TREE);
                return;
            }
        }
    }

    public void ExitCutTreeState()
    {
    }
    #endregion

    #region RunAway
    public void EnterRunAwayState()
    {
        if (m_Animator != null)
        {
            m_Animator.ResetTrigger("Walking");
            m_Animator.SetTrigger("Chased");
            m_Animator.ResetTrigger("Cutting");
        }
    }

    public void UpdateRunAwayState()
    {
        m_Dir = transform.position - m_NearestVolunteerChasing.position;
        transform.position += (Vector3)m_Dir.normalized * m_RunSpeed * Time.deltaTime;

        //if out of boundary, set inactive
        if (OutOfBoundary())
        {
            gameObject.SetActive(false);
            return;
        }

        //if volunteer is too far, just ignore and go back to other mode
        if (Vector2.SqrMagnitude(m_Dir) > m_MinSafeDist * m_MinSafeDist)
        {
            ChangeState(States.MOVE_TO_TREE);
        }
    }
    #endregion

    #region GetOutOfMap State
    public void EnterGetOutOfMap()
    {
        //check if its already out of boundary
        if (OutOfBoundary())
        {
            gameObject.SetActive(false);
            return;
        }

        //randomize a location outside of the map
        Vector2Int gridLocationOutside = GetALocationOutSideMap();
        m_Destination = MapManager.Instance.GetGridPosToWorld(gridLocationOutside);
        m_Dir = m_Destination - (Vector2)transform.position;
        m_Dir.Normalize();

        if (m_Animator != null)
        {
            m_Animator.ResetTrigger("Walking");
            m_Animator.SetTrigger("Chased");
            m_Animator.ResetTrigger("Cutting");
        }
    }

    public void UpdateGetOutOfMap()
    {
        transform.position += (Vector3)m_Dir * m_Speed * Time.deltaTime;

        if (OutOfBoundary())
        {
            gameObject.SetActive(false);
            return;
        }
    }
    #endregion

    public bool OutOfBoundary()
    {
        //check if out of bounds
        Vector2 pos = transform.position;

        Bounds bound = MapManager.Instance.m_MapBoundary;
        return (pos.x - m_OutOfBoundsOffset >= bound.max.x
            || pos.y - m_OutOfBoundsOffset >= bound.max.y
            || pos.x + m_OutOfBoundsOffset <= bound.min.x
            || pos.y + m_OutOfBoundsOffset <= bound.min.y);
    }

    public Vector2Int GetALocationOutSideMap()
    {
        BoundsInt mapBoundary = MapManager.Instance.m_MapBoundaryGridNo;

        //TODO:: change the 1 to something better
        int sideToSpawn = Random.Range(0, 4);
        Vector2Int spawnGridPos = Vector2Int.zero;
        switch (sideToSpawn)
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

        return spawnGridPos;
    }

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
        Gizmos.DrawWireSphere(transform.position, m_MinSafeDist);
        Gizmos.DrawWireSphere(transform.position, m_OutOfBoundsOffset);
    }
#endif
}
