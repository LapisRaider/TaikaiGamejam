using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Camera")]
    public float m_CameraMoveSpeed = 10.0f;
    public Bounds m_CameraBoundary;
    Camera m_MainCamera;

    void Start()
    {
        m_MainCamera = Camera.main;
        m_CameraBoundary = MapManager.Instance.m_MapBoundary;
        float cameraHeight = 2.0f * m_MainCamera.orthographicSize;
        float cameraWidth = cameraHeight * m_MainCamera.aspect;

        m_CameraBoundary.min = new Vector3(m_CameraBoundary.min.x + cameraWidth / 2.0f, m_CameraBoundary.min.y + m_MainCamera.orthographicSize, m_CameraBoundary.min.z);
        m_CameraBoundary.max = new Vector3(m_CameraBoundary.max.x - cameraWidth / 2.0f, m_CameraBoundary.max.y - m_MainCamera.orthographicSize, m_CameraBoundary.max.z);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    #region Camera
    public void UpdateCamera()
    {
        Vector3 dir = CameraInput();
        Vector3 newCameraTargetPos = m_MainCamera.transform.position + (dir * m_CameraMoveSpeed * Time.deltaTime);
        newCameraTargetPos = new Vector3(Mathf.Clamp(newCameraTargetPos.x, m_CameraBoundary.min.x, m_CameraBoundary.max.x), Mathf.Clamp(newCameraTargetPos.y, m_CameraBoundary.min.y, m_CameraBoundary.max.y), newCameraTargetPos.z);

        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, newCameraTargetPos, 1.0f);
    }

    public Vector2 CameraInput()
    {
        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
            dir.x = -1.0f;
        else if (Input.GetKey(KeyCode.D))
            dir.x = 1.0f;

        if (Input.GetKey(KeyCode.W))
            dir.y = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            dir.y = -1.0f;

        return dir;
    }
    #endregion
}
