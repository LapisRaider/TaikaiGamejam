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
    PlantTree m_NearestTree = null;
    Vector2 m_Destination = Vector2.zero;


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
                break;
            case States.CUTTING_TREE:
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

                if (dist < m_MinNoOfGrid * m_MinNoOfGrid)
                {
                    //the nearest tree
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
        //if (Vector2.Distance(m_Destination, (Vector2)transform.position) <= 2.0f)
        //    return;

        //transform.position += (Vector3)(m_Dir * 5.0f * Time.deltaTime);
    }

    public void ExitFindTreeState()
    {

    }
    #endregion
}
