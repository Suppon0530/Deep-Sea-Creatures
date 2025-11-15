using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Creaturemgrdata", menuName = "Creaturemgrdata")]

// 生物データを管理するクラス
public class Creaturemgrdata : ScriptableObject
{
    [SerializeField] private Creaturedata creaturedata;    // 生物データの内部クラス

    private GameObject[] creature_prefab;    // 生物オブジェクトのプレハブ
    private Sprite[] creature;    // 生物スプライト
    private Sprite[] creature_frame;    // 生物フレームのスプライト
    private int[] score;    // 生物オブジェクトの単スコア
    private int[] id;    // 生物オブジェクトの識別番号

    private List<int> lists = new List<int>();    // 生物の数のリスト
    private const int creature_num = 20;    // 生物の数


    // 生物データの初期化
    public void SetCreature()
    {
        // 各変数、5つまで取得
        creature_prefab = new GameObject[5];
        creature = new Sprite[5];
        creature_frame = new Sprite[5];
        score = new int[5];
        id = new int[5];

        // リストに0~19を格納
        for(int i = 0; i < creature_num; i++) {
            lists.Add(i);
        }

        // リストから数字を5つ重複なしで取得
        for(int i = 0; i < 5; i++) {
            int random = UnityEngine.Random.Range(0, creature_num - i);    // 0以上creature_num - 1未満

            creature_prefab[i] = creaturedata.GetCreature_Prefab(lists[random]);
            creature[i] = creaturedata.GetCreature(lists[random]);
            creature_frame[i] = creaturedata.GetCreature_Frame(lists[random]);
            score[i] = creaturedata.GetScore(lists[random]);
            id[i] = creaturedata.GetID(lists[random]);

            lists.RemoveAt(random);
        }

        // 選択されたプレハブの回転角をリセット
        for(int i = 0; i < 5; i++) {
            creature_prefab[i].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    // 生物オブジェクトのプレハブを返却する関数
    public GameObject GetCreature_Prefab(int num)
    {
        return this.creature_prefab[num];
    }

    // 生物スプライトを返却する関数
    public Sprite GetCreature(int num)
    {
        return this.creature[num];
    }

    // 生物フレームスプライトを返却する関数
    public Sprite GetCreature_Frame(int num)
    {
        return this.creature_frame[num];
    }

    // 生物オブジェクトのスコアを返却する関数
    public int GetScore(int num)
    {
        return this.score[num];
    }

    // 生物オブジェクトの識別番号を返却する関数
    public int GetID(int num)
    {
        return this.id[num];
    }


    // 生物データの内部クラス
    [System.Serializable]
    private class Creaturedata {

        [SerializeField] private GameObject[] creature_prefab;    // 生物オブジェクトのプレハブ
        [SerializeField] private Sprite[] creature;    // 生物スプライト
        [SerializeField] private Sprite[] creature_frame;    // 生物フレームのスプライト
        [SerializeField] private int[] score;    // 生物オブジェクトの単スコア
        [SerializeField] private int[] id;    // 生物オブジェクトの識別番号


        // 生物オブジェクトのプレハブを返却する関数
        public GameObject GetCreature_Prefab(int num)
        {
            return this.creature_prefab[num];
        }

        // 生物スプライトを返却する関数
        public Sprite GetCreature(int num)
        {
            return this.creature[num];
        }

        // 生物フレームスプライトを返却する関数
        public Sprite GetCreature_Frame(int num)
        {
            return this.creature_frame[num];
        }

        // 生物オブジェクトのスコアを返却する関数
        public int GetScore(int num)
        {
            return this.score[num];
        }

        // 生物オブジェクトの識別番号を返却する関数
        public int GetID(int num)
        {
            return this.id[num];
        }
    }
}