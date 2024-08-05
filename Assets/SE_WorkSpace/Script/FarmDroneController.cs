using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FarmDroneController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference droneMovement;
    [SerializeField]
    private InputActionReference droneRotate;
    [SerializeField]
    private InputActionReference toggleCanvasAction; // X 버튼 

    public Canvas vrCanvas;
    public RawImage camera;
    public RawImage camera2;

    private Rigidbody rb;
    private Vector3 lastPosition;
    private Vector3 initialPosition; // 초기 위치를 저장할 변수
    private Quaternion initialRotation; // 초기 회전을 저장할 변수
    private float moveSpeed = 15f; // 기본 이동 속도
    private bool isRawImage1Active = true; // 현재 활성화된 Raw Image
    private bool isCanvasActive = false; // 캔버스 활성화

    private void OnEnable()
    {
        droneMovement.action.Enable();
        droneRotate.action.Enable();
        toggleCanvasAction.action.Enable();
    }

    private void OnDisable()
    {
        droneMovement.action.Disable();
        droneRotate.action.Disable();
        toggleCanvasAction.action.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        initialPosition = transform.position; // 초기 위치 저장
        initialRotation = transform.rotation; // 초기 회전 저장

        if (vrCanvas != null)
        {
            vrCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //movement
        Vector3 dir = droneMovement.action.ReadValue<Vector3>();
        Vector3 new3dir = new Vector3(-dir.x, dir.z, dir.y);
        gameObject.transform.Translate(new3dir * 5 * moveSpeed * Time.deltaTime);

        //rotation
        float rotate = droneRotate.action.ReadValue<float>();
        gameObject.transform.Rotate(Vector3.forward, rotate * 100 * Time.deltaTime);

        //toggle
        if (toggleCanvasAction.action.triggered)
        {
            ToggleRawImages();
        }

        // 매 프레임마다 마지막 위치를 업데이트합니다.
        lastPosition = transform.position;
    }

    private void ToggleRawImages()
    {
        if (vrCanvas != null)
        {
            if (!isCanvasActive) // 캔버스 비활성화 된 경우
            {
                vrCanvas.gameObject.SetActive(true); // 캔버스를 활성화하고 첫번째 카메라 이미지 표시
                camera.gameObject.SetActive(true); // 첫번째 카메라 활성화
                camera2.gameObject.SetActive(false); // 두번째 카메라 비활성화
                isRawImage1Active = true; // 첫번째 카메라가 활성화되었음을 기록
                isCanvasActive = true; // 캔버스가 활성화되었음을 기록
            }
            else // 캔버스 활성화 된 경우
            {    
                if (isRawImage1Active) // 첫번째 이미지 활성화된 경우
                {
                    camera.gameObject.SetActive(false); // 첫번째 카메라 비활성화
                    camera2.gameObject.SetActive(true); // 두번째 카메라 활성화
                }
                else // 두번째 카메라 활성화된 경우
                {
                    camera2.gameObject.SetActive(false); // 두번째 카메라 비활성화
                    vrCanvas.gameObject.SetActive(false); // 캔버스 비활성화
                    isCanvasActive = false;
                }
                isRawImage1Active = !isRawImage1Active; // 활성화된 카메라 이미지 전환
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 바닥과의 충돌을 감지하여 속도를 0으로 설정
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 특정 트리거와의 충돌을 감지하여 속도를 0으로 설정
        if (other.CompareTag("Fence"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
