using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public GameObject itemPrefab; // 스폰할 아이템 프리팹
    public string itemName; // 아이템 이름

    public GameObject SpawnItem(Vector3 position, Quaternion rotation)
    {
        return Instantiate(itemPrefab, position, rotation); // 생성된 오브젝트 반환
    }
}
