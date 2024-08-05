using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LoadManager : MonoBehaviour
{
    public GameObject objectPrefab; // 오브젝트를 생성할 프리팹
    public SaveManager saveManager; // 생성된 오브젝트를 관리할 SaveManager

    private void Start()
    {
        Load(); // 씬 시작 시 자동으로 로드
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            // 기존 오브젝트 삭제
            foreach (var obj in saveManager.objectsToSave)
            {
                if (obj.CompareTag("Saveable"))
                {
                    Destroy(obj);
                }
            }
            saveManager.objectsToSave.Clear();

            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            foreach (var data in saveData.objects)
            {
                bool objectExists = false;

                // 이미 존재하는 오브젝트인지 확인
                foreach (var obj in saveManager.objectsToSave)
                {
                    if (obj.name == data.name)
                    {
                        objectExists = true;
                        obj.transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
                        obj.transform.rotation = new Quaternion(data.rotationX, data.rotationY, data.rotationZ, data.rotationW);
                        break;
                    }
                }

                // 오브젝트가 존재하지 않으면 새로 생성
                if (!objectExists)
                {
                    GameObject obj = Instantiate(objectPrefab, new Vector3(data.positionX, data.positionY, data.positionZ),
                                                 new Quaternion(data.rotationX, data.rotationY, data.rotationZ, data.rotationW));
                    obj.name = data.name;
                    saveManager.AddObjectToSave(obj);
                }
            }

            Debug.Log("Data Loaded");
        }
        else
        {
            Debug.LogWarning("Save file not found");
        }
    }
}
