using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// タイマークラス
public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer_text;
    [SerializeField] private GameObject score;
    [SerializeField] private GameObject screenshot;
    [SerializeField] private Image count_timer;
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] AudioClip count_sound;    // カウントダウン音
    [SerializeField] AudioClip end_sound;    // カウントダウン終了音

    private float count_time;    // ゲームカウントのタイマー
    private int countdown;    // ゲームのカウント
    private int count_state;
    private bool stop;
    private AudioSource audiosource;


    void Start()
    {
        count_timer.gameObject.SetActive(true);
        count_time = 4.0f;
        countdown = 3;
        count_state = 0;

        audiosource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // カウントの状態で場合分け
        switch(count_state) {
            // スタートまでのカウント
            case 0:
                if(count_time >= 1) {
                    if(countdown == (int)count_time) {
                        audiosource.PlayOneShot(count_sound);
                        countdown--;
                    }
                    count_time -= Time.deltaTime;
                    if(count_time >= 1) {
                        count.text = "" + (int)count_time;
                    }
                }
                else {
                    if(countdown == (int)count_time) {
                        audiosource.PlayOneShot(end_sound);
                        countdown = 10;
                    }
                    count_time = 61;
                    count_state = 1;
                    count_timer.gameObject.SetActive(false);
                }
                break;
            // ゲームカウント
            case 1:
                if(count_time >= 1) {
                    if(count_time < 11 && count_time >= 1) {
                        if(countdown == (int)count_time) {
                            audiosource.PlayOneShot(count_sound);
                            countdown--;
                        }
                    }
                    count_time -= Time.deltaTime;
                    timer_text.text = "TIME : " + (int)count_time;
                }
                else {
                    timer_text.text = "TIME : 0";
                    if(countdown == (int)count_time) {
                        audiosource.PlayOneShot(end_sound);
                        countdown = 0;
                    }
                    count_time = 4;
                    count_state = 2;
                    count_timer.gameObject.SetActive(true);
                }
                break;
            // ゲーム終了までのカウント
            case 2:
                if(count_time >= 1) {
                    count_time -= Time.deltaTime;
                    if(count_time >= 1) {
                        count.text = "" + (int)count_time;
                    }
                }
                else {
                    score.GetComponent<PlayerMgr>().SaveMyScore();
                    count_timer.gameObject.SetActive(false);
                    screenshot.GetComponent<ScreenShotMgr>().ScreenShot();
                    SceneManager.LoadScene("Result");
                }
                break;
            // case 1と同様
            default:
                break;
        }
    }

    public bool EndTimer()
    {
        if(countdown == 0) {
            return true;
        }
        return false;
    }
}
