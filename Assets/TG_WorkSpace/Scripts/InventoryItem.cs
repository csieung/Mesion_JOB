using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public GameObject itemPrefab; // ������ ������ ������
    public string itemName; // ������ �̸�

    public GameObject SpawnItem(Vector3 position, Quaternion rotation)
    {
        return Instantiate(itemPrefab, position, rotation); // ������ ������Ʈ ��ȯ
    }
}
