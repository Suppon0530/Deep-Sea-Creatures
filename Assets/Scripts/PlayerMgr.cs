using UnityEngine;
using TMPro;

// プレイヤークラス
public class PlayerMgr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score_text;
    [SerializeField] private Creaturemgrdata creaturedata;

    private GameObject scoremgr;    // スコア管理オブジェクト
    private GameObject gallerymgr;    // ギャラリー管理オブジェクト

    private int score;    // スコア
    private int bonus_score_0;    // ボーナススコア0
    private int bonus_score_1;    // ボーナススコア1
    private GalleryMgr script;

    // Start is called before the first frame update
    void Start()
    {
        scoremgr = GameObject.Find("ScoreMgr");
        gallerymgr = GameObject.Find("GalleryMgr");

        score = 0;
        score_text.text = "SCORE : " + score;

        if(gallerymgr != null) {
            script = gallerymgr.GetComponent<GalleryMgr>();
        }
    }

    // スコアを加算する関数
    public void AddScore(int num)
    {
        this.score += num;
        score_text.text = "SCORE : " + score;
    }

    // スコアを保存する関数
    public void SaveMyScore()
    {
        // 生物インスタンスのタグからインスタンス数を取得
        GameObject[] creature_0 = GameObject.FindGameObjectsWithTag("creature_0");
        GameObject[] creature_1 = GameObject.FindGameObjectsWithTag("creature_1");
        GameObject[] creature_2 = GameObject.FindGameObjectsWithTag("creature_2");
        GameObject[] creature_3 = GameObject.FindGameObjectsWithTag("creature_3");
        GameObject[] creature_4 = GameObject.FindGameObjectsWithTag("creature_4");

        // 生物インスタンスの数に応じてボーナス点
        bonus_score_0 = (creature_0.Length + creature_1.Length + creature_2.Length + creature_3.Length +creature_4.Length) * 50;

        // 全ての種類の生物インスタンスが使用されている場合、ボーナス点
        for(int i = 0; i < 5; i++) {
            if(creature_0.Length > i && creature_1.Length > i && creature_2.Length > i && creature_3.Length > i && creature_4.Length > i) {
                bonus_score_1 = 3000 * (i + 1);
            }
        }

        // ハイスコアを保存
        if(scoremgr != null) {
            scoremgr.GetComponent<ScoreMgr>().SetHighScore(score + bonus_score_0 + bonus_score_1, bonus_score_0, bonus_score_1);
        }

        // 生物データを保存
        script.SetCreatureData(creaturedata.GetID(0), creature_0.Length);
        script.SetCreatureData(creaturedata.GetID(1), creature_1.Length);
        script.SetCreatureData(creaturedata.GetID(2), creature_2.Length);
        script.SetCreatureData(creaturedata.GetID(3), creature_3.Length);
        script.SetCreatureData(creaturedata.GetID(4), creature_4.Length);
    }
}
