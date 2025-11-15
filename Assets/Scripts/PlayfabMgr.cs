using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

// PlayFabのログインクラス
public class PlayfabMgr : MonoBehaviour
{    
    private bool _shouldCreateAccount;    //アカウントを作成するかどうか   
    private string _customID;    //ログイン時に使うID

    private static bool DontDestroy = false;
    private ScoreMgr playerdata;    // スコアデータ

    // Start is called before the first frame update
    public void Start()
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

        // スコアデータを取得
        GameObject dontDestroyObject = GameObject.Find("ScoreMgr");
        if (dontDestroyObject != null) {
            playerdata = dontDestroyObject.GetComponent<ScoreMgr>();
        }

        Login();    // ログイン処理   
    }

    // ログインを実行する関数
    private void Login()
    {
        LoadCustomID();    // カスタムIDを取得する処理
    }

    // ログイン成功
    private void OnLoginSuccess(LoginResult result)
    {
        // アカウントを作成しようとしたのに、IDが既に使われていて、出来なかった場合
        if (_shouldCreateAccount == true && result.NewlyCreated == false) {
            Debug.LogWarning("CustomId :" +_customID+ "は既に使われています。");
            Login();    // ログインしなおし
            return;
        }

        // アカウント新規作成できたらIDを保存
        if (result.NewlyCreated == true) {
            SaveCustomID();
            Debug.Log("新規作成成功");
        }

        Debug.Log("ログイン成功!!");
    }

    // ログイン失敗
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("PlayFabのログインに失敗\n" + error.GenerateErrorReport());
    }

    // カスタムIDを取得する関数
    private void LoadCustomID()
    {
        // IDをスコアデータから取得
        string id = playerdata.GetID();

        // idの中身が空の文字列("")の場合は、カスタムIDを生成するフラグをtrue
        if(id == "") {
            _shouldCreateAccount = true;
        }
        else {
            _shouldCreateAccount = false;
            _customID = id;
        }

        GenerateCustomID();    // カスタムIDを生成する処理
    }

    // IDを保存する関数
    private void SaveCustomID()
    {
        playerdata.SetID(_customID);
    }

    //ユニークな文字列をGuidを使用し生成する処理
    //https://docs.microsoft.com/ja-jp/dotnet/api/system.guid.tostring?redirectedfrom=MSDN&view=netframework-4.8#System_Guid_ToString_System_String_
    private void GenerateCustomID()
    {
        // idの中身がない場合、文字列を新規作成
        if (_shouldCreateAccount == true) {
            // Guidの構造体生成
            Guid guid = Guid.NewGuid();
                
            _customID = guid.ToString("N");    // 書式指定子はNを指定　詳細は「Guid.ToString メソッド」のドキュメント参照
        }
        
        var request = new LoginWithCustomIDRequest { CustomId = _customID, CreateAccount = _shouldCreateAccount };//補足　既にアカウントが作成されており、CreateAccountがtrueになっていてもエラーにはならない
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

}