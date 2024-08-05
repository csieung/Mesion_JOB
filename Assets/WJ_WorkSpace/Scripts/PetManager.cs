using UnityEngine;

public class PetManager : MonoBehaviour
{
    public Pet pet; // 인스펙터에서 설정할 ScriptableObject

    void Start()
    {
        if (pet != null)
        {
            Debug.Log($"펫 이름: {pet.petName}");
            Debug.Log($"레벨: {pet.level}");
            Debug.Log($"경험치: {pet.experience}");
            Debug.Log($"호감도: {pet.affection}");
        }
    }

    public void IncreaseAffection(float amount)
    {
        if (pet != null)
        {
            pet.affection += amount;
            Debug.Log($"{pet.petName}의 호감도가 {amount}만큼 증가했습니다.");
        }
    }
}
