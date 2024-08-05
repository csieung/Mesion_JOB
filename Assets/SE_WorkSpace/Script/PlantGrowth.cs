using System.Collections;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public GameObject[] growthStages;
    private int currentStage = 0;
    private bool isGrowing = false;

    void Start()
    {
        // ��� ���� �ܰ� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
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
