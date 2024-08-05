using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    [SerializeField]
    public Transform xrRigCamera;

    void Update()
    {
        // ī�޶� �� ��ġ�� ���� ����Ʈ�� ����
        transform.position = xrRigCamera.position + xrRigCamera.forward * 1.5f;
        transform.rotation = xrRigCamera.rotation;
    }
}
