using UnityEngine;
using UnityEngine.UI;

// リザルト画面クラス
public class ResultScreenShot : MonoBehaviour
{
    [SerializeField] private Image image;    // リザルトイメージ

    private ScoreMgr playerdata;    // スコアデータ

    // Start is called before the first frame update
    void Start()
    {
        // スコアデータからリザルトイメージを取得
        GameObject dontDestroyObject = GameObject.Find("ScoreMgr");
        if (dontDestroyObject != null)
        {
            playerdata = dontDestroyObject.GetComponent<ScoreMgr>();
        }

        image.GetComponent<Image>().sprite = playerdata.GetSprite();
    }
}
