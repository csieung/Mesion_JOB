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
    private InputActionReference toggleCanvasAction; // X ��ư 

    public Canvas vrCanvas;
    public RawImage camera;
    public RawImage camera2;

    private Rigidbody rb;
    private Vector3 lastPosition;
    private Vector3 initialPosition; // �ʱ� ��ġ�� ������ ����
    private Quaternion initialRotation; // �ʱ� ȸ���� ������ ����
    private float moveSpeed = 15f; // �⺻ �̵� �ӵ�
    private bool isRawImage1Active = true; // ���� Ȱ��ȭ�� Raw Image
    private bool isCanvasActive = false; // ĵ���� Ȱ��ȭ

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
        initialPosition = transform.position; // �ʱ� ��ġ ����
        initialRotation = transform.rotation; // �ʱ� ȸ�� ����

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

        // �� �����Ӹ��� ������ ��ġ�� ������Ʈ�մϴ�.
        lastPosition = transform.position;
    }

    private void ToggleRawImages()
    {
        if (vrCanvas != null)
        {
            if (!isCanvasActive) // ĵ���� ��Ȱ��ȭ �� ���
            {
                vrCanvas.gameObject.SetActive(true); // ĵ������ Ȱ��ȭ�ϰ� ù��° ī�޶� �̹��� ǥ��
                camera.gameObject.SetActive(true); // ù��° ī�޶� Ȱ��ȭ
                camera2.gameObject.SetActive(false); // �ι�° ī�޶� ��Ȱ��ȭ
                isRawImage1Active = true; // ù��° ī�޶� Ȱ��ȭ�Ǿ����� ���
                isCanvasActive = true; // ĵ������ Ȱ��ȭ�Ǿ����� ���
            }
            else // ĵ���� Ȱ��ȭ �� ���
            {    
                if (isRawImage1Active) // ù��° �̹��� Ȱ��ȭ�� ���
                {
                    camera.gameObject.SetActive(false); // ù��° ī�޶� ��Ȱ��ȭ
                    camera2.gameObject.SetActive(true); // �ι�° ī�޶� Ȱ��ȭ
                }
                else // �ι�° ī�޶� Ȱ��ȭ�� ���
                {
                    camera2.gameObject.SetActive(false); // �ι�° ī�޶� ��Ȱ��ȭ
                    vrCanvas.gameObject.SetActive(false); // ĵ���� ��Ȱ��ȭ
                    isCanvasActive = false;
                }
                isRawImage1Active = !isRawImage1Active; // Ȱ��ȭ�� ī�޶� �̹��� ��ȯ
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �ٴڰ��� �浹�� �����Ͽ� �ӵ��� 0���� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ư�� Ʈ���ſ��� �浹�� �����Ͽ� �ӵ��� 0���� ����
        if (other.CompareTag("Fence"))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
