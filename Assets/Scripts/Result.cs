using UnityEngine;
using UnityEngine.SceneManagement;

// リザルトクラス
public class Result : MonoBehaviour
{
    // 戻るボタン選択時の処理
    public void OnReturnButtonClick()
    {
        SceneManager.LoadScene("Title");
    }
}
