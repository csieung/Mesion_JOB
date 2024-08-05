using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpraySeedController : MonoBehaviour
{
    public GameObject seedPrefab; // ���� ������
    public Transform seedOrigin; // ���� �л� ���� ��ġ
    public float sprayForce = 10f; // ���� �л� ��
    public int createSeed = 5; // �ʴ� ������ ���� ����

    private bool isSpraying = false;

    [SerializeField]
    private InputActionReference spraySeedAction;

    private void OnEnable()
    {
        spraySeedAction.action.Enable();
        spraySeedAction.action.performed += OnSpraySeedPerformed;
        spraySeedAction.action.canceled += OnSpraySeedCanceled;
    }

    private void OnDisable()
    {
        spraySeedAction.action.Disable();
        spraySeedAction.action.performed -= OnSpraySeedPerformed;
        spraySeedAction.action.canceled -= OnSpraySeedCanceled;
    }

    private void OnSpraySeedPerformed(InputAction.CallbackContext context)
    {
        StartSpraying();
    }

    private void OnSpraySeedCanceled(InputAction.CallbackContext context)
    {
        StopSpraying();
    }

    private void StartSpraying()
    {
        isSpraying = true;
        StartCoroutine(SpraySeed());
    }

    private void StopSpraying()
    {
        isSpraying = false;
        StopAllCoroutines();
    }

    private IEnumerator SpraySeed()
    {
        while (isSpraying)
        {
            for (int i = 0; i < 3; i++) // �� ���� ���� ����� ����
            {
                Vector3 randomDirection = GetRandomDirection();
                CreateSeedDrop(randomDirection);
            }

            yield return new WaitForSeconds(1f / createSeed);
        }
    }
    private Vector3 GetRandomDirection()
    {
        float randomAngle = Random.Range(-15f, 15f);
        Quaternion rotation = Quaternion.Euler(randomAngle, 0, 0);
        return rotation * Vector3.down; // �Ʒ������� ���� ����
    }

    private void CreateSeedDrop(Vector3 direction)
    {
        GameObject droplet = Instantiate(seedPrefab, seedOrigin.position, Quaternion.identity);
        Rigidbody rb = droplet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * sprayForce, ForceMode.VelocityChange);
        }

        Destroy(droplet, 10); // 10�� �� ���� ����
    }
}
