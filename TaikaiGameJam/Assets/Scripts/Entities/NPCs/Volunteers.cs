using UnityEngine;

public class Volunteers : MonoBehaviour
{
    [Header("Info")]
    public SpriteRenderer m_SpriteRenderer;
    public SpriteRenderer m_ItemSpriteRenderer;
    public Animator m_Animator;

    [Header("Movement")]
    public Vector2 m_MinMaxSpeed = new Vector2(0.1f, 1.0f);
    public float m_BoundaryRadius = 2.0f; //for when the NPCs are about to hit the boundary
    public float m_RotationSpeed = 0.9f;

    [Header("Idle State")]
    public float m_ChangeDirTime = 2.0f;
    public float m_RotationAngle = 10.0f;
    public float m_BorderRotationOffset = 4.0f;
    public float m_CheckCanPlantTime = 0.5f; //reduce the amount of function calls to check whether theres space available to plant
    float m_LastPlantingTime = 3.0f; //how long it takes before the volunteer can plant again
    [Tooltip("Radius, so if x is 1, it will show abv and below by 1")]
    public Vector2Int m_PlantTreeSearchRadius = new Vector2Int(1, 1);
    float m_ChangeDirTimeTracker = 0.0f;
    float m_CheckCanPlantTimeTracker = 0.0f;
    float m_LastPlantTimeTracker = 0.0f;

    [Header("Move To Location State")]
    Vector2 m_Destination = Vector2.zero;
    public float m_StopDistance = 2.0f;

    [Header("Chase state")]
    public float m_ChaseSpeed = 3.0f;
    EvilPeople m_EvilPersonChasing; 

    [Header("Plant Tree State")]
    public Vector2Int m_PlantTreeGridPos = Vector2Int.zero;
    float m_PlantingTime = 0.0f;
    float m_PlantingTimeTracker = 0.0f;

    public enum States
    {
        IDLE, //move ard freely
        MOVE_TO_FREE_SPACE, //move to a space to plant tree
        PLANT_TREE, //plant a tree down
        CHASE //for chasing away evil business people
    }

    States m_CurrentState = States.IDLE;
    Vector2 m_MoveDir = Vector2.zero;
    Vector2 m_NextDir = Vector2.zero;
    float m_Speed = 0.0f;

    public void Awake()
    {
        m_Speed = Random.Range(m_MinMaxSpeed.x, m_MinMaxSpeed.y);
        m_BorderRotationOffset = m_BorderRotationOffset * Mathf.Deg2Rad;
    }

    public void OnEnable()
    {
        m_CurrentState = States.IDLE;
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
                UpdatePlantTreeState();
                break;
            case States.CHASE:
                UpdateChaseState();
                break;
        }

        m_SpriteRenderer.sortingOrder = (int)(transform.position.y * -100);
        if (m_ItemSpriteRenderer != null)
            m_ItemSpriteRenderer.sortingOrder = (int)(transform.position.y * -100);

        if (m_Animator != null)
            m_Animator.SetFloat("HorizontalVelocity", m_MoveDir.x);
    }

    public void ChangeState(States newState)
    {
        States oldState = m_CurrentState;
        m_CurrentState = newState;

        //check current state and exit
        switch (oldState)
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
    }

    public bool CheckCanChase()
    {
        return m_CurrentState == States.IDLE || m_CurrentState == States.MOVE_TO_FREE_SPACE;
    }

    public void ChaseEvilPerson(EvilPeople evilperson)
    {
        m_EvilPersonChasing = evilperson;
        ChangeState(States.CHASE);
    }

    #region IdleState
    public void EnterIdleState()
    {
        if (m_Animator != null)
            m_Animator.SetTrigger("Idle");

        m_MoveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        m_NextDir = m_MoveDir;

        m_CheckCanPlantTimeTracker = 0.0f;
        m_ChangeDirTimeTracker = 0.0f;
        //update new direction
        ChangeDirection();
    }

    public void UpdateIdleState()
    {
        //make NPC wander around the map, every few seconds will change direction
        m_ChangeDirTimeTracker += Time.deltaTime;
        if (m_ChangeDirTimeTracker > m_ChangeDirTime)
            ChangeDirection();

        m_CheckCanPlantTimeTracker += Time.deltaTime;
        m_CheckCanPlantTimeTracker = Mathf.Min(m_CheckCanPlantTimeTracker, m_CheckCanPlantTime + 1.0f);

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

        //check last planted a tree timing to prevent NPC from planting at similar positions
        m_LastPlantTimeTracker += Time.deltaTime;
        m_LastPlantTimeTracker = Mathf.Min(m_LastPlantTimeTracker, m_LastPlantingTime + 1.0f);
        if (m_LastPlantTimeTracker > m_LastPlantingTime)
        {
            //check if theres anything in the inventory
            if (!GameStats.Instance.CheckIsInventoryEmpty())
            {
                if (m_CheckCanPlantTimeTracker >= m_CheckCanPlantTime)
                {
                    if (CheckIfCanPlantTrees())
                        ChangeState(States.MOVE_TO_FREE_SPACE);

                    m_CheckCanPlantTimeTracker = 0.0f;
                }
            }
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

        if (m_Animator != null)
            m_Animator.SetTrigger("Idle");
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
    public void EnterPlantTreeState()
    {
        //call the plant functiom, this functiom will handle choosing of the plant
        //check if inventory is available first
        m_PlantingTimeTracker = 0.0f;
        m_LastPlantTimeTracker = 0.0f;

        if (!GameStats.Instance.CheckIsInventoryEmpty())
        {
            //get the time needed to plant this plant
            m_PlantingTime = MapManager.Instance.Plant(m_PlantTreeGridPos);
        }
        else
        {
            ChangeState(States.IDLE);
            return;
        }

        if (m_Animator != null)
            m_Animator.SetTrigger("Planting");
    }

    public void UpdatePlantTreeState()
    {
        m_PlantingTimeTracker += Time.deltaTime;
        if (m_PlantingTimeTracker > m_PlantingTime)
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
        if (m_Animator != null)
            m_Animator.SetTrigger("Chasing");
    }

    public void UpdateChaseState()
    {
        m_MoveDir = m_EvilPersonChasing.transform.position - transform.position;
        m_MoveDir.Normalize();

        transform.position += (Vector3)m_MoveDir * m_ChaseSpeed * Time.deltaTime;

        //if evil person is inactive, meaning out of the map
        if (!m_EvilPersonChasing.gameObject.activeSelf)
        {
            ChangeState(States.IDLE);
        }
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
