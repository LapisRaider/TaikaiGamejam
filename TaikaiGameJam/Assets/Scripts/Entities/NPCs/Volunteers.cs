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
    public Vector2 m_MinMaxSpeed = new Vector2(0.1f,1.0f);


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
    float m_Speed = 0.0f;

    public void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();

        m_Speed = Random.Range(m_MinMaxSpeed.x, m_MinMaxSpeed.y);
    }

    public void Update()
    {
        switch(m_CurrentState)
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
    }

    public void UpdateIdleState()
    {
        //make NPC wander around the map, every few seconds will change direction

    }

    public void ExitIdleState()
    {

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
}
