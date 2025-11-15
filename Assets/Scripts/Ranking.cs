using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

// ランキングクラス
public class Ranking : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI my_score_text;    // マイスコアテキスト
    [SerializeField] private TextMeshProUGUI[] score_text;    // オンラインランキングのハイスコアテキストの配列

    private ScoreMgr playerdata;    // スコアデータ
    private int high_score;    // ハイスコア(自身の)

    // Start is called before the first frame update
    void Start()
    {
        // スコアデータを取得
        GameObject dontDestroyObject = GameObject.Find("ScoreMgr");
        if (dontDestroyObject != null)
        {
            playerdata = dontDestroyObject.GetComponent<ScoreMgr>();
        }

        // 自身のハイスコアが設定されていれば、PlayFabから自身のスコアを取得
        high_score = playerdata.GetHighScore();
        if(high_score != 0) {
            GetPlayFabMyScore();
        }  

        GetPlayFabHighScore();    // PlayFabからスコアランキングを取得
    }

    // PlayFabのランキングを取得する関数
    private void GetPlayFabHighScore()
    {
        // GetLeaderboardRequestのインスタンスを生成
        var request = new GetLeaderboardRequest
        {
            StatisticName   = "Ranking",    //ランキング名(統計情報名)
            StartPosition   = 0,    //何位以降のランキングを取得するか
            MaxResultsCount = 10    //ランキングデータを何件取得するか(最大100)
        };

        //ランキング(リーダーボード)を取得
        Debug.Log($"ランキング(リーダーボード)の取得開始");
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    // ランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log($"ランキング(リーダーボード)の取得に成功しました");

        // result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        int i = 0;
        // foreach文で取得
        foreach (var entry in result.Leaderboard) {
            int displayPosition = entry.Position + 1;    // ランク
            string space = "00";    // ランクの前につける文字列
            // ランクの桁により、spaceを変更
            if(9 < displayPosition && displayPosition < 100) {
                space = "0";
            }
            else if(displayPosition >= 100) {
                space = null;
            }
            score_text[i].text = space + displayPosition + ". " + entry.DisplayName + "\n         score : " + entry.StatValue;
            i++;
        }
    }

    // ランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError($"ランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }

    // PlayFabから自身のスコア近辺のランキングを取得する関数(今回は自身のランキングのみ)
    private void GetPlayFabMyScore()
    {
        // GetLeaderboardAroundPlayerRequestのインスタンスを生成
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName   = "Ranking",    //ランキング名(統計情報名)
            MaxResultsCount = 1    //自分を含め前後何件取得するか
        };

        // 自分の順位周辺のランキング(リーダーボード)を取得
        Debug.Log($"自分の順位周辺のランキング(リーダーボード)の取得開始");
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardAroundPlayerSuccess, OnGetLeaderboardAroundPlayerFailure);
    }

    // 自分の順位周辺のランキング(リーダーボード)の取得成功
    private void OnGetLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
    {
        Debug.Log($"自分の順位周辺のランキング(リーダーボード)の取得に成功しました");

        // result.Leaderboardに各順位の情報(PlayerLeaderboardEntry)が入っている
        // 上記と同様
        foreach (var entry in result.Leaderboard) {
            int displayPosition = entry.Position + 1;
            string space = "00";
            if(9 < displayPosition && displayPosition < 100) {
                space = "0";
            }
            else if(displayPosition >= 100) {
                space = null;
            }
            my_score_text.text = "RANK. " + space + displayPosition + " : " + entry.DisplayName + "\nHIGH_SCORE : " + high_score + "\nSCORE : " + entry.StatValue;
        }
    }

    //自分の順位周辺のランキング(リーダーボード)の取得失敗
    private void OnGetLeaderboardAroundPlayerFailure(PlayFabError error)
    {
        Debug.LogError($"自分の順位周辺のランキング(リーダーボード)の取得に失敗しました\n{error.GenerateErrorReport()}");
    }
}
