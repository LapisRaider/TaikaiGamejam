using System.Collections;
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
    }

    public void UpdateFindTreeState()
    {
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
    }

    public void ExitCutTreeState()
    {
        //if tree is still alive, reset the tree health
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, m_CutTreeDist);
    }
#endif
}
