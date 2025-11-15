using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// タイトルクラス
public class Title : MonoBehaviour
{
    [SerializeField] private Creaturemgrdata creaturedata;    // 生物データ
    [SerializeField] private Image ranking;    // ランキング画面
    [SerializeField] private Image gallery;    // ギャラリー画面
    [SerializeField] private Image manual;    // 遊び方画面
    [SerializeField] private Image setting;    // 設定画面


    // プレイボタン選択時の処理
    public void OnPlayButtonClick()
    {
        creaturedata.SetCreature();
        SceneManager.LoadScene("Game");
    }

    // ランキングボタン選択時の処理
    public void OnRankingButtonClick()
    {
        ranking.gameObject.SetActive(true);
    }

    // 戻るボタン選択時の処理(ランキング画面)
    public void OnRankingReturnClick()
    {
        ranking.gameObject.SetActive(false);
    }

    // ギャラリーボタン選択時の処理
    public void OnGalleryButtonClick()
    {
        gallery.gameObject.SetActive(true);
    }

    // 戻るボタン選択時の処理(ギャラリー画面)
    public void OnGalleryReturnClick()
    {
        gallery.gameObject.SetActive(false);
    }

    // 遊び方ボタン選択時の処理
    public void OnManualButtonClick()
    {
        manual.gameObject.SetActive(true);
    }

    // 戻るボタン選択時の処理(遊び方画面)
    public void OnManualReturnClick()
    {
        manual.gameObject.SetActive(false);
    }

    // 設定ボタン選択時の処理
    public void OnSettingButtonClick()
    {
        setting.gameObject.SetActive(true);
    }

    // 設定ボタン選択時の処理(設定画面)
    public void OnSettingReturnClick()
    {
        setting.gameObject.GetComponent<Login>().LeaveName();
        setting.gameObject.SetActive(false);
    }
}