using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LoadManager : MonoBehaviour
{
    public GameObject objectPrefab; // ������Ʈ�� ������ ������
    public SaveManager saveManager; // ������ ������Ʈ�� ������ SaveManager

    private void Start()
    {
        Load(); // �� ���� �� �ڵ����� �ε�
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/saveFile.json";
        if (File.Exists(path))
        {
            // ���� ������Ʈ ����
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

                // �̹� �����ϴ� ������Ʈ���� Ȯ��
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

                // ������Ʈ�� �������� ������ ���� ����
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
