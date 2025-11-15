using System.Collections.Generic;
using UnityEngine;
using System.IO;
using PlayFab;
using PlayFab.ClientModels;

// スコア管理クラス
public class ScoreMgr : MonoBehaviour
{
    // jsonファイルへのハイスコア情報クラス
    [System.Serializable]
    public class ScoreData
    {
        public string id;    // ユーザごとの一意のID
        public string name;    // ユーザ名
        public int score;    // トータルスコア
        public int high_score;    // ハイスコア
        public int bonus_score_0;    // ボーナススコア
        public int bonus_score_1;    // ボーナススコア
    }

    private ScoreData scoredata;    // スコアデータ
    private string filePath;    // ファイルパス
    private static bool DontDestroy = false;
    private Sprite result_image;    // リザルトのイメージ

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

        filePath = Path.Combine(Application.persistentDataPath, "my_score.json");
        LoadScore();
    }

    //  ハイスコアを保存する関数
    public void SetHighScore(int num_0, int num_1, int num_2)
    {
        if(scoredata.high_score < num_0) {
            scoredata = new ScoreData()
            {
                id = scoredata.id, 
                name = scoredata.name, 
                score = num_0, 
                high_score = num_0, 
                bonus_score_0 = num_1, 
                bonus_score_1 = num_2
            };
        }
        else {
            scoredata = new ScoreData()
            {
                id = scoredata.id, 
                name = scoredata.name, 
                score = num_0, 
                high_score = scoredata.high_score, 
                bonus_score_0 = num_1, 
                bonus_score_1 = num_2
            };
        }
        SaveScore();
        SavePlayFabScore();
    }

    // スコアデータをロードする関数
    private void LoadScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            scoredata = JsonUtility.FromJson<ScoreData>(json);
        }
        else
        {
            // データが存在しない場合、デフォルトのデータを作成
            scoredata = new ScoreData()
            {
                id = "", 
                name = "", 
                score = 0, 
                high_score = 0, 
                bonus_score_0 = 0, 
                bonus_score_1 = 0
            };
            SaveScore();
        }
    }

    // スコアデータを保存する関数
    private void SaveScore()
    {
        string json = JsonUtility.ToJson(scoredata);
        File.WriteAllText(filePath, json);
    }

    // スコアを返却する関数
    public int GetTotal()
    {
        if (scoredata != null)
        {
            return scoredata.score;
        }

        return 0;
    }

    // スコアを返却する関数
    public int GetScore()
    {
        if (scoredata != null)
        {
            return (scoredata.score - scoredata.bonus_score_0 - scoredata.bonus_score_1);
        }

        return 0;
    }

    // スコアを返却する関数
    public int GetBonus0()
    {
        if (scoredata != null)
        {
            return scoredata.bonus_score_0;
        }

        return 0;
    }
    // スコアを返却する関数
    public int GetBonus1()
    {
        if (scoredata != null)
        {
            return scoredata.bonus_score_1;
        }

        return 0;
    }

    // ハイスコアを返却する関数
    public int GetHighScore()
    {
        if (scoredata != null)
        {
            return scoredata.high_score;
        }

        return 0;
    }

    // IDを取得する関数
    public void SetID(string tmp)
    {
        // IDを変更
        if(scoredata != null) {
            scoredata.id = tmp;
        }

        SaveScore();
    }

    // IDを返却する関数
    public string GetID()
    {
        if(scoredata != null) {
            return scoredata.id;
        }

        return "";
    }

    // 名前を取得する関数
    public void SetName(string tmp)
    {
        // 名前を変更
        if(scoredata != null) {
            scoredata.name = tmp;
        }

        SaveScore();
    }

    // 名前を返却する関数
    public string GetName()
    {
        if(scoredata != null) {
            return scoredata.name;
        }

        return "";
    }

    // リザルトスプライトを設定する関数
    public void SetSprite(Sprite sprite)
    {
        result_image = sprite;
    }

    // リザルトスプライトを返却する関数
    public Sprite GetSprite()
    {
        return result_image;
    }

    // PlayFabにハイスコアを保存する関数
    private void SavePlayFabScore()
    {
        //UpdatePlayerStatisticsRequestのインスタンスを生成
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Ranking",    //ランキング名(統計情報名)
                    Value = scoredata.high_score,    //スコア(int)
                }
            }
        };

        // ユーザ名の更新
        Debug.Log($"スコア(統計情報)の更新開始");
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsSuccess, OnUpdatePlayerStatisticsFailure);
    }

    // スコア(統計情報)の更新成功
    private void OnUpdatePlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log($"スコア(統計情報)の更新が成功しました");
    }

    // スコア(統計情報)の更新失敗
    private void OnUpdatePlayerStatisticsFailure(PlayFabError error)
    {
        Debug.LogError($"スコア(統計情報)更新に失敗しました\n{error.GenerateErrorReport()}");
    }
}
