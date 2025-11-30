using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

// インタースティシャル広告のAdmobクラス
public class AdMobMgr : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private GameObject bannerObj;
    private static bool Dontdestroy = false;
    private static int count = 0;    // インタースティシャル広告を表示するまでの回数

    void Awake()
    {
        if(!Dontdestroy) {
            DontDestroyOnLoad(this);
            Dontdestroy = true;
        }
        else {
            Destroy(gameObject);
            return;
        }

        // AdMob SDKを初期化
        MobileAds.Initialize((InitializationStatus initStatus) => { });
    }
    
    void Start()
    {
        RequestBanner();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        if(scene.name == "Result") LoadInterstitialAd();
    }

    // インタースティシャル広告を読み込む関数
    private void LoadInterstitialAd()
    {
        #if UNITY_ANDROID
            // string adUnitId_in = "ca-app-pub-3940256099942544/1033173712";    // テスト広告
            string adUnitId_in = "ca-app-pub-3771226114317990/9784204290";    // 本番広告
        #elif UNITY_IPHONE
            // string adUnitId_in = "ca-app-pub-3940256099942544/4411468910";    // テスト広告
            string adUnitId_in = "ca-app-pub-3771226114317990/4425712621";    // 本番広告
        #else
            string adUnitId_in = "unexpected_platform";
        #endif
            if(count == 1) {
                if(interstitial != null) {
                    interstitial.Destroy();
                    interstitial = null;
                }
                InterstitialAd.Load(adUnitId_in, new AdRequest(),
                (InterstitialAd ad, LoadAdError loadAdError) =>
                {
                    if (loadAdError != null) {
                        // Interstitial ad failed to load with error
                        Debug.Log("Interstitial ad failed to load with error: ");
                        bannerView.Show();
                        return;
                    }
                    else if (ad == null) {
                        // Interstitial ad failed to load.
                        bannerView.Show();
                        return;
                    }
                    ad.OnAdFullScreenContentClosed += () =>
                    {
                        HandleOnAdClosed();
                        bannerView.Show();
                    };
                    ad.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        HandleOnAdClosed();
                        bannerView.Show();
                    };

                    interstitial = ad;

                    bannerView.Hide();
                    
                    ShowInterstitialAd();
                    count = 0;
                });
            }
            else {
                count = 1;
            }
    }

    // インタースティシャル広告を削除する関数
    private void HandleOnAdClosed()
    {
        if(interstitial != null) {
            interstitial.Destroy();
            interstitial = null;
        }
    }

    // インタースティシャル広告を表示する関数
    private void ShowInterstitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd()) {
            interstitial.Show();
        }
        else {
            Debug.Log("Interstitial Ad not load");
        }
    }

    // バナーのリクエスト関数
    private void RequestBanner()
    {
        #if UNITY_ANDROID
            // string adUnitId_ba = "ca-app-pub-3940256099942544/6300978111"; // テスト用広告ユニットID
            string adUnitId_ba = "ca-app-pub-3771226114317990/5386987228";    // 本番用広告ユニットID
        #elif UNITY_IPHONE
            // string adUnitId_ba = "ca-app-pub-3940256099942544/2934735716"; // テスト用広告ユニットID
            string adUnitId_ba = "ca-app-pub-3771226114317990/3315357655";    // 本番用広告ユニットID
        #else
            string adUnitId_ba = "unexpected_platform";
        #endif
            if(bannerView != null) {
                bannerView.Destroy();
                bannerView = null;
            }
            // Create a 320x50 banner at the bottom of the screen.
            bannerView = new BannerView(adUnitId_ba, AdSize.Banner, AdPosition.Top);

            LoadBanner();
    }

    // バナーのロード関数
    private void LoadBanner()
    {
        if(bannerView == null) RequestBanner();

        // Create an empty ad request.
        AdRequest request = new AdRequest();

        // Load the banner with the request.
        bannerView.LoadAd(request);

        if(bannerObj = GameObject.Find("BANNER(Clone)")) DontDestroyOnLoad(bannerObj);
    }
}
