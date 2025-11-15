using UnityEngine;
using TMPro;

// リザストスコアクラス
public class ResultScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI total_text;
    [SerializeField] private TextMeshProUGUI score_text;
    [SerializeField] private TextMeshProUGUI bonus0_text;
    [SerializeField] private TextMeshProUGUI bonus1_text;
    [SerializeField] private TextMeshProUGUI high_score_text;

    private ScoreMgr playerdata;
    private int total;
    private int score;
    private int bonus0;
    private int bonus1;
    private int high_score;

    // Start is called before the first frame update
    void Start()
    {
        GameObject dontDestroyObject = GameObject.Find("ScoreMgr");
        if (dontDestroyObject != null)
        {
            playerdata = dontDestroyObject.GetComponent<ScoreMgr>();
        }

        total = playerdata.GetTotal();
        score = playerdata.GetScore();
        bonus0 = playerdata.GetBonus0();
        bonus1 = playerdata.GetBonus1();
        high_score = playerdata.GetHighScore();

        total_text.text = "  TOTAL : " + total;
        score_text.text = "    -base score : " + score;
        bonus0_text.text = "    -num of creatures : " + (bonus0 / 50) + " × " +  50;
        bonus1_text.text = "    -all kinds of creatures : " + (bonus1 / 3000) + " × " + 3000;
        high_score_text.text = "  HIGH_SCORE : " + high_score;
    }
}
