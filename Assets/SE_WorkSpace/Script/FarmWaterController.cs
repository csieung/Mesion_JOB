using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FarmWaterController : MonoBehaviour
{
    public ParticleSystem waterSprayParticle; // 물 분사 파티클 시스템
    public Transform WaterOrigin; // 물 분사 시작 위치

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
            // 파티클 시스템을 비활성화하여 처음에는 보이지 않도록 설정
            waterSprayParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void Update()
    {
        if (WaterOrigin != null && waterSprayParticle != null)
        {
            // 물 분사 파티클 시스템의 위치를 물 분사 시작 위치로 설정
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
