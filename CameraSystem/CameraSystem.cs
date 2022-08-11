using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField]
    private float fieldOfViewMax = 50;
    [SerializeField]
    private float fieldOfViewMin = 10;
    [SerializeField]
    private float followOffsetMin = 5;
    [SerializeField]
    private float followOffsetMax = 50;
    [SerializeField]
    private float followOffsetMinY = 10;
    [SerializeField]
    private float followOffsetMaxY = 50;
    [SerializeField] ScrollingMode scrollingMode  = ScrollingMode.FieldOfView;


    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private float targetFieldOfView = 50;
    private Vector3 followOffset;


    void Awake()
    {
        followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }
    
    void Update()
    {
        HandelCameraMovement();

        HandelCameraMovementEdgeScrolling();

        HandelCameraMovementDragPan();

        HandelCameraRotation();

        HandleCameraZoom_FieldOfView();

        HandleCameraZoom_MoveForward();

        HandleCameraZoom_LowerY();
    }

    void HandelCameraMovement()
    {
        Vector3 inputDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;


        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 50;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void HandelCameraMovementEdgeScrolling()
    {
        Vector3 inputDir = Vector3.zero;

        int edgeScrollSize = 20;

        if (Input.mousePosition.x < edgeScrollSize)
        {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollSize)
        {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollSize)
        {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollSize)
        {
            inputDir.z = +1f;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 20;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void HandelCameraMovementDragPan()
    {
        Vector3 inputDir = Vector3.zero;


        if (Input.GetMouseButtonDown(2))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(2))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 1f;
            inputDir.x = mouseMovementDelta.x * dragPanSpeed;
            inputDir.z = mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 20;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void HandelCameraRotation()
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        float rotateSpeed = 100;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    void HandleCameraZoom_FieldOfView()
    {
        if (scrollingMode != ScrollingMode.FieldOfView) return;

        if (Input.mouseScrollDelta.y > 0)
        {
            targetFieldOfView -= 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFieldOfView += 5;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, fieldOfViewMin, fieldOfViewMax);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.FieldOfView =
            Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * zoomSpeed);

    }

    void HandleCameraZoom_MoveForward()
    {
        if (scrollingMode != ScrollingMode.MoveForvard) return;

        Vector3 zoomDir = followOffset.normalized;

        float zoomAmount = 3f;
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset -= zoomDir * zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset += zoomDir * zoomAmount;
        }

        if (followOffset.magnitude < followOffsetMin)
        {
            followOffset = zoomDir * followOffsetMin;
        }
        if (followOffset.magnitude > followOffsetMax)
        {
            followOffset = zoomDir * followOffsetMax;
        }

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, zoomSpeed * Time.deltaTime);

    }

    void HandleCameraZoom_LowerY()
    {
        if (scrollingMode != ScrollingMode.LowerY) return;

        float zoomAmount = 3f;
        if(Input.mouseScrollDelta.y > 0)
        {
            followOffset.y -= zoomAmount; 
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset.y += zoomAmount;
        }

        followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, zoomSpeed * Time.deltaTime);

    }
}

public enum ScrollingMode
{
    FieldOfView,
    MoveForvard,
    LowerY
}
