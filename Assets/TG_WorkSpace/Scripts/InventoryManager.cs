using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventoryItem[] inventoryItems;
    public Transform spawnPoint; // XR Rig의 카메라 앞 위치
    public SaveManager saveManager; // SaveManager 추가

    void Start()
    {
        foreach (var item in inventoryItems)
        {
            Button itemButton = item.GetComponent<Button>();
            itemButton.onClick.AddListener(() => OnItemClicked(item));
        }
    }

    public void OnItemClicked(InventoryItem item)
    {
        GameObject newObj = item.SpawnItem(spawnPoint.position, spawnPoint.rotation);
        saveManager.AddObjectToSave(newObj); // 생성된 오브젝트를 SaveManager에 추가
    }
}
