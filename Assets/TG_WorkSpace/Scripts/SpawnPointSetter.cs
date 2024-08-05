using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    [SerializeField]
    public Transform xrRigCamera;

    void Update()
    {
        // 카메라 앞 위치를 스폰 포인트로 설정
        transform.position = xrRigCamera.position + xrRigCamera.forward * 1.5f;
        transform.rotation = xrRigCamera.rotation;
    }
}
