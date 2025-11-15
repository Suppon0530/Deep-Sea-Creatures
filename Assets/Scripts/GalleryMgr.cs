using System.Collections.Generic;
using UnityEngine;
using System.IO;

// ギャラリー管理クラス
public class GalleryMgr : MonoBehaviour
{
    // jsonファイルへのギャラリークラス
    [System.Serializable]
    public class GalleryData
    {
        public List<CreatureData> CreatureDatas = new List<CreatureData>();    // 生物データ
    }

    // 生物データクラス
    [System.Serializable]
    public class CreatureData
    {
        public string key;    // 生物の識別番号
        public int num;    // 生物の捕獲数
    }

    private GalleryData gallerydata;    // ギャラリーデータ
    private string filePath;    // ファイルパス
    private static bool DontDestroy = false;

    // Start()より前に実行される関数
    void Awake()
    {
        // DontDestroyがfalseであれば、DontDestroyOnLoad()
        if (!DontDestroy)
        {
            DontDestroyOnLoad(this);
            DontDestroy = true;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        filePath = Path.Combine(Application.persistentDataPath, "gallery.json");
        LoadGallery();
    }

    // 生物データを保存する関数
    public void SetCreatureData(int id, int num)
    {
        string key = GetCreatureKey(id);
        CreatureData record = FindCreatureRecord(key);
        
        if (record == null) {
            record = new CreatureData()
            {
                key = key,
                num = num
            };
            gallerydata.CreatureDatas.Add(record);
        }
        else {
            record.num += num;
        }

        SaveGallery();
    }

    // ギャラリーデータをロードする関数
    private void LoadGallery()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gallerydata = JsonUtility.FromJson<GalleryData>(json);
        }
        else
        {
            gallerydata = new GalleryData();
            SaveGallery();
        }
    }

    // ギャラリーデータを保存する関数
    private void SaveGallery()
    {
        string json = JsonUtility.ToJson(gallerydata);
        File.WriteAllText(filePath, json);
    }

    // 生物データを返却する関数
    public int GetCreatureData(int id)
    {
        string key = GetCreatureKey(id);
        CreatureData record = FindCreatureRecord(key);
        return record != null ? record.num : 0;
    }

    // 生物データの識別番号を返却する関数
    private CreatureData FindCreatureRecord(string key)
    {
        return gallerydata.CreatureDatas.Find(record => record.key == key);
    }

    // 生物データの識別番号をint->stringに変更する関数
    private string GetCreatureKey(int key)
    {
        return $"creature_{key}";
    }
}
