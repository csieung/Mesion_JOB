using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmWaterController : MonoBehaviour
{
    public ParticleSystem waterSprayParticle; // �� �л� ��ƼŬ �ý���
    public Transform WaterOrigin; // �� �л� ���� ��ġ

    [SerializeField]
    private InputActionReference FarmWaterAaction;

    private void OnEnable()
    {
        FarmWaterAaction.action.Enable();
        FarmWaterAaction.action.performed += OnSprayWaterPerformed;
        FarmWaterAaction.action.canceled += OnSprayWaterCanceled;
    }

    private void OnDisable()
    {
        FarmWaterAaction.action.Disable();
        FarmWaterAaction.action.performed -= OnSprayWaterPerformed;
        FarmWaterAaction.action.canceled -= OnSprayWaterCanceled;
    }

    private void Start()
    {
        if (waterSprayParticle != null)
        {
            // ��ƼŬ �ý����� ��Ȱ��ȭ�Ͽ� ó������ ������ �ʵ��� ����
            waterSprayParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void Update()
    {
        if (WaterOrigin != null && waterSprayParticle != null)
        {
            // �� �л� ��ƼŬ �ý����� ��ġ�� �� �л� ���� ��ġ�� ����
            waterSprayParticle.transform.position = WaterOrigin.position;
            waterSprayParticle.transform.rotation = WaterOrigin.rotation * Quaternion.Euler(180, 0, 0);
        }

    }

    private void OnSprayWaterPerformed(InputAction.CallbackContext context)
    {
        StartSpraying();
    }

    private void OnSprayWaterCanceled(InputAction.CallbackContext context)
    {
        StopSpraying();
    }

    private void StartSpraying()
    {
        if (waterSprayParticle != null && !waterSprayParticle.isPlaying)
        {
            waterSprayParticle.Play();
        }
    }

    private void StopSpraying()
    {
        if (waterSprayParticle != null && waterSprayParticle.isPlaying)
        {
            waterSprayParticle.Stop();
        }
    }
}
