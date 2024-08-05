using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint; // XR Rig의 카메라 앞 위치
    public bool spawnAsChildren = false; // Default value, modify as needed
    public int spawnOptionIndex = 0; // Default value, modify as needed

    // Method to spawn an object
    public GameObject SpawnObject(Vector3 position, Quaternion rotation)
    {
        GameObject newObj = Instantiate(objectPrefab, position, rotation);

        if (spawnAsChildren)
        {
            newObj.transform.parent = this.transform;
        }

        return newObj;
    }

    // Example method for randomizing spawn options
    public void RandomizeSpawnOption()
    {
        // Implement your randomization logic here
        spawnOptionIndex = Random.Range(0, 10); // Example range, modify as needed
    }
}
