using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volunteers : MonoBehaviour
{
    //will walk around the map
    //will check from mapmanager whether the space is free to plant
    //plant tree
    //move on

    //different states, IDLE mode, where theyll just wander about and search for areas for trees
    //plant trees mode, go to the destination and plant a tree
    //back to idle mode
    [Header("Movement")]
    public Vector2 m_MinMaxSpeed = new Vector2(0.1f, 1.0f);
    public Vector2 m_MinMaxWalkOffset = new Vector2(-0.5f, 0.5f);
    public float m_BoundaryRadius = 2.0f; //for when the NPCs are about to hit the boundary
    public float m_RotationSpeed = 0.9f;

    [Header("Idle State")]
    public float m_ChangeDirTime = 2.0f;
    public float m_RotationAngle = 10.0f;
    public float m_BorderRotationOffset = 4.0f;
    float m_ChangeDirTimeTracker = 0.0f;

    public enum States
    {
        IDLE, //move ard freely
        MOVE_TO_FREE_SPACE, //move to a space to plant tree
        PLANT_TREE, //plant a tree down
        CHASE //for chasing away evil business people
    }

    CharacterController m_CharacterController;
    Animator m_Animator;

    States m_CurrentState = States.IDLE;
    Vector2 m_MoveDir = Vector2.zero;
    Vector2 m_NextDir = Vector2.zero;
    float m_Speed = 0.0f;

    public void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();

        m_Speed = Random.Range(m_MinMaxSpeed.x, m_MinMaxSpeed.y);
        m_BorderRotationOffset = m_BorderRotationOffset * Mathf.Deg2Rad;

        EnterIdleState();
    }

    public void Update()
    {
        switch (m_CurrentState)
        {
            case States.IDLE:
                UpdateIdleState();
                break;
            case States.MOVE_TO_FREE_SPACE:
                break;
            case States.PLANT_TREE:
                break;
            case States.CHASE:
                break;
        }
    }

    public void ChangeState(States newState)
    {
        //check current state and exit
        switch (m_CurrentState)
        {
            case States.IDLE:
                ExitIdleState();
                break;
            case States.MOVE_TO_FREE_SPACE:
                ExitMoveToFreeSpaceState();
                break;
            case States.PLANT_TREE:
                ExitPlantTreeState();
                break;
            case States.CHASE:
                ExitChaseState();
                break;
        }

        //check new state and enter
        switch (newState)
        {
            case States.IDLE:
                EnterIdleState();
                break;
            case States.MOVE_TO_FREE_SPACE:
                EnterMoveToFreeSpaceState();
                break;
            case States.PLANT_TREE:
                EnterPlantTreeState();
                break;
            case States.CHASE:
                EnterChaseState();
                break;
        }

        m_CurrentState = newState;
    }

    #region IdleState
    public void EnterIdleState()
    {
        //change animation accordingly


        m_MoveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        m_NextDir = m_MoveDir;
        //update new direction
        ChangeDirection();
    }

    public void UpdateIdleState()
    {
        //make NPC wander around the map, every few seconds will change direction
        m_ChangeDirTimeTracker += Time.deltaTime;
        if (m_ChangeDirTimeTracker > m_ChangeDirTime)
            ChangeDirection();

        m_MoveDir.Normalize();


        Vector3 checkPos = transform.position + (Vector3)(m_Speed * m_NextDir);
        if (CheckOutsideBoundary(checkPos))
        {
            //rotate away from the wall
            Bounds bound = MapManager.Instance.m_MapBoundary;
            float angle = 0.0f;
            if (checkPos.x + m_BoundaryRadius >= bound.max.x || checkPos.x - m_BoundaryRadius <= bound.min.x)
            {
                //wall is towards the right or left
                angle = Mathf.Acos(Vector2.Dot(m_NextDir, Vector2.up));
            }

            if (checkPos.y + m_BoundaryRadius >= bound.max.y || checkPos.y - m_BoundaryRadius <= bound.min.y)
            {
                //wall is up or down
                angle = Mathf.Acos(Vector2.Dot(m_NextDir, Vector2.right));
            }

            if (angle > Mathf.PI / 2)
                angle = Mathf.PI - angle;


            float sin = Mathf.Sin(angle + m_BorderRotationOffset);
            float cos = Mathf.Cos(angle + m_BorderRotationOffset);

            m_NextDir = new Vector2(cos * m_NextDir.x - sin * m_NextDir.y, sin * m_NextDir.x + cos * m_NextDir.y);
            m_NextDir.Normalize();
        }

        m_MoveDir = Vector2.Lerp(m_MoveDir, m_NextDir, m_RotationSpeed * Time.deltaTime);

        //Debug.DrawLine(transform.position, transform.position + ((Vector3)m_MoveDir * m_Speed), Color.red);
        //Debug.DrawLine(transform.position, transform.position + ((Vector3)m_NextDir * m_Speed), Color.green);

        Vector3 newPos = transform.position + (Vector3)(m_Speed * m_MoveDir * Time.deltaTime);
        transform.position = newPos;
    }

    public void ExitIdleState()
    {

    }

    public void ChangeDirection()
    {
        float angle = Random.Range(-m_RotationAngle, m_RotationAngle);
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Cos(angle * Mathf.Deg2Rad);

        m_NextDir = new Vector2(cos * m_MoveDir.x - sin * m_MoveDir.y, sin * m_MoveDir.x + cos * m_MoveDir.y);
        m_NextDir.Normalize();

        m_ChangeDirTimeTracker = 0.0f;

        //Debug.Log(m_NextDir);
    }
    #endregion

    #region MoveToFreeSpaceState
    public void EnterMoveToFreeSpaceState()
    {

    }

    public void UpdateMoveToFreeSpaceState()
    {

    }

    public void ExitMoveToFreeSpaceState()
    {

    }
    #endregion

    #region PlantTreeState
    public void EnterPlantTreeState()
    {

    }

    public void UpdatePlantTreeState()
    {

    }

    public void ExitPlantTreeState()
    {

    }
    #endregion

    #region ChaseState
    public void EnterChaseState()
    {

    }

    public void UpdateChaseState()
    {

    }

    public void ExitChaseState()
    {

    }
    #endregion

    public bool CheckOutsideBoundary(Vector2 newPos)
    {
        //make sure new position is within boundaries
        Bounds bound = MapManager.Instance.m_MapBoundary;
        return (newPos.x + m_BoundaryRadius >= bound.max.x
            || newPos.y + m_BoundaryRadius >= bound.max.y
            || newPos.x - m_BoundaryRadius <= bound.min.x
            || newPos.y - m_BoundaryRadius <= bound.min.y);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, m_BoundaryRadius);
    }
#endif
}
