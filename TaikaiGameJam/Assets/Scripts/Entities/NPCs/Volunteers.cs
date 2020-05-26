using UnityEngine;

public class Volunteers : MonoBehaviour
{
    [Header("Info")]
    SpriteRenderer m_SpriteRenderer;

    [Header("Movement")]
    public Vector2 m_MinMaxSpeed = new Vector2(0.1f, 1.0f);
    public float m_BoundaryRadius = 2.0f; //for when the NPCs are about to hit the boundary
    public float m_RotationSpeed = 0.9f;

    [Header("Idle State")]
    public float m_ChangeDirTime = 2.0f;
    public float m_RotationAngle = 10.0f;
    public float m_BorderRotationOffset = 4.0f;
    [Tooltip("Radius, so if x is 1, it will show abv and below by 1")]
    public Vector2Int m_PlantTreeSearchRadius = new Vector2Int(1, 1);
    float m_ChangeDirTimeTracker = 0.0f;

    [Header("Move To Location State")]
    public Vector2 m_Destination = Vector2.zero;

    [Header("Plant Tree State")]
    public Vector2Int m_PlantTreeGridPos = Vector2Int.zero;
    public float m_StopDistance = 2.0f;

    float m_PlantTiming = 0.0f; //TEMP

    public enum States
    {
        IDLE, //move ard freely
        MOVE_TO_FREE_SPACE, //move to a space to plant tree
        PLANT_TREE, //plant a tree down
        CHASE //for chasing away evil business people
    }

    Animator m_Animator;

    States m_CurrentState = States.IDLE;
    Vector2 m_MoveDir = Vector2.zero;
    Vector2 m_NextDir = Vector2.zero;
    float m_Speed = 0.0f;

    public void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

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
                UpdateMoveToFreeSpaceState();
                break;
            case States.PLANT_TREE:
                break;
            case States.CHASE:
                break;
        }

        m_SpriteRenderer.sortingOrder = (int)(transform.position.y * -100);
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
    float tempTimer = 0.0f;



    public void EnterIdleState()
    {
        //change animation accordingly

        m_MoveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        m_NextDir = m_MoveDir;
        //update new direction
        ChangeDirection();

        tempTimer = 0.0f;
    }

    public void UpdateIdleState()
    {
        //make NPC wander around the map, every few seconds will change direction
        m_ChangeDirTimeTracker += Time.deltaTime;
        if (m_ChangeDirTimeTracker > m_ChangeDirTime)
            ChangeDirection();

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
        m_MoveDir.Normalize();
        //Debug.DrawLine(transform.position, transform.position + ((Vector3)m_MoveDir * m_Speed), Color.red);
        //Debug.DrawLine(transform.position, transform.position + ((Vector3)m_NextDir * m_Speed), Color.green);

        Vector3 newPos = transform.position + (Vector3)(m_Speed * m_MoveDir * Time.deltaTime);
        transform.position = newPos;

        //TODO:: temp TIMER, CHANGE TO SOMETHING ELSE
        tempTimer += Time.deltaTime;
        if (tempTimer > 3.0f)
        {
            if (CheckIfCanPlantTrees())
                ChangeState(States.MOVE_TO_FREE_SPACE);
        }
    }

    public bool CheckIfCanPlantTrees()
    {
        //check if position nearby got empty space
        Vector2Int gridCurrentPos = MapManager.Instance.GetWorldToGridPos(transform.position);
        for (int y = -m_PlantTreeSearchRadius.y; y <= m_PlantTreeSearchRadius.y; ++y)
        {
            for (int x = -m_PlantTreeSearchRadius.x; x <= m_PlantTreeSearchRadius.x; ++x)
            {
                //able to plant tree at the location
                m_PlantTreeGridPos = gridCurrentPos + new Vector2Int(x, y);
                if (MapManager.Instance.CheckCanPlantTree(m_PlantTreeGridPos))
                {
                    m_Destination = MapManager.Instance.GetGridPosToWorld(m_PlantTreeGridPos);
                    return true;
                }
            }
        }

        return false;
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
    }
    #endregion

    #region MoveToFreeSpaceState
    public void EnterMoveToFreeSpaceState()
    {
        m_MoveDir = m_Destination - (Vector2)transform.position;
        m_MoveDir.Normalize();
    }

    public void UpdateMoveToFreeSpaceState()
    {
        transform.position += (Vector3)(m_Speed * Time.deltaTime * m_MoveDir);

        if (Vector2.SqrMagnitude(m_Destination - (Vector2)transform.position) < m_StopDistance * m_StopDistance)
        {
            ChangeState(States.PLANT_TREE);
        }
    }

    public void ExitMoveToFreeSpaceState()
    {

    }
    #endregion

    #region PlantTreeState
    float otherTempTimer = 0.0f; //TODO:: remove this


    public void EnterPlantTreeState()
    {
        MapManager.Instance.Plant(m_PlantTreeGridPos, Plant_Types.TREES);

        otherTempTimer = 0.0f;
    }

    public void UpdatePlantTreeState()
    {
        otherTempTimer += Time.deltaTime;
        if (otherTempTimer > 4)
        {
            ChangeState(States.IDLE);
        }
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
