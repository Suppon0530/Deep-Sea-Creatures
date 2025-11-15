using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

// ログインクラス
public class Login : MonoBehaviour
{
    [SerializeField] private GameObject input;    // InputFieldのゲームオブジェクト

    private ScoreMgr playerdata;    // スコアデータ
    private TMP_InputField inputfield;    // InputField
    private string username;    // ユーザ名

    // Start is called before the first frame update
    void Start()
    {
        // スコアデータを取得
        GameObject dontDestroyObject = GameObject.Find("ScoreMgr");
        if (dontDestroyObject != null)
        {
            playerdata = dontDestroyObject.GetComponent<ScoreMgr>();
        }

        // ユーザ名の取得・表示
        inputfield = input.GetComponent<TMP_InputField>();
        inputfield.text = playerdata.GetName();

        // 初回ログインであるかの場合分け(""文字列は初回)
        if(inputfield.text == "") {
            this.gameObject.SetActive(true);
        }
        else {
            this.gameObject.SetActive(false);
        }
    }

    public void OnSaveButton()
    {
        // 空白で保存した場合、ユーザ名"Unknown", それ以外入力文字列
        if(inputfield.text == "") {
            username = "Unknown";
        }
        else {
            username = inputfield.text;
        }

        // ユーザ名を指定して、UpdateUserTitleDisplayNameRequestのインスタンスを生成
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        // ユーザ名の更新
        Debug.Log($"ユーザ名の更新開始");
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateUserNameSuccess, OnUpdateUserNameFailure);
    }

    // ユーザ名の更新成功
    private void OnUpdateUserNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        // result.DisplayNameに更新した後のユーザ名が入ってる
        Debug.Log($"ユーザ名の更新が成功しました : {result.DisplayName}");
        playerdata.SetName(username);
        this.gameObject.SetActive(false);
    }

    // ユーザ名の更新失敗
    private void OnUpdateUserNameFailure(PlayFabError error)
    {
        Debug.LogError($"ユーザ名の更新に失敗しました\n{error.GenerateErrorReport()}");
        if(inputfield.text.Length < 3) {
            inputfield.text = "at least 3 characters";
        }
        else {
            inputfield.text = "already in use";
        }
    }

    // ユーザ名を残す関数
    public void LeaveName()
    {
        inputfield.text = playerdata.GetName();
    }
}
