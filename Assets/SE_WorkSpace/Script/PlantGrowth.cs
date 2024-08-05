using System.Collections;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public GameObject[] growthStages;
    private int currentStage = 0;
    private bool isGrowing = false;

    void Start()
    {
        // 모든 성장 단계 오브젝트를 비활성화합니다.
        foreach (GameObject stage in growthStages)
        {
            stage.SetActive(false);
        }
    }
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("WaterParticle") && !isGrowing)
        {
            StartCoroutine(GrowPlant());
        }
    }

    IEnumerator GrowPlant()
    {
        isGrowing = true;
        while (currentStage < growthStages.Length - 1)
        {
            yield return new WaitForSeconds(2.0f);
            growthStages[currentStage].SetActive(false);
            currentStage++;
            growthStages[currentStage].SetActive(true);
        }
        isGrowing = false;
    }
}
