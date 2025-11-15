using UnityEngine;

// 生物クラス
public class Creature : MonoBehaviour
{
    private int score;    // 生物のスコア

    private GameObject scores;    // スコアのゲームオブジェクト
    private PlayerMgr script_score;    // スコアオブジェクトのスクリプト
    private GameObject net;    // 網のゲームオブジェクト
    private bool collided = false;    // 一度でも衝突したかどうかの変数
    private bool releasing = false;    // 生物オブジェクトが落下しているかどうかの変数


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーオブジェクトを取得
        scores = GameObject.Find("PlayerMgr");
        if(scores != null) {
            script_score = scores.GetComponent<PlayerMgr>();
        }

        // ネットオブジェクトを取得
        net = GameObject.Find("Obj_NetFrame");
        if(net != null) {
            this.gameObject.transform.SetParent(net.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 一定の高さまで落ちた場合の処理
        if(transform.position.y < -6) {
            if(collided) {
                script_score.AddScore(-score);
            }
            Destroy(this.gameObject);
        } 
    }

    // 衝突した場合にスコアを加算する関数
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 一度も衝突していない かつ 生物オブジェクトが落下している場合に処理
        if(!collided && releasing) {
            script_score.AddScore(score);
            collided = true;
        }
    }

    // 生物オブジェクトの落下を設定する関数
    public void SetReleasing()
    {
        releasing = true;
    }

    // 生物オブジェクトの単スコアを設定する関数
    public void SetScore(int num)
    {
        score = num;
    }
}
