using Management;
using UnityEngine;
using GoogleMobileAds.Api;

//Admob manager
public class AdmobManager : GameManager<AdmobManager> {
    //Admob ID
    private string bannerID = "ca-app-pub-2782384029579894/5207437794";
    private string frontID = "ca-app-pub-2782384029579894/8558954476";
    //Test ID
    private string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    private string frontTestID = "ca-app-pub-3940256099942544/1033173712";

    private BannerView bv = null; //Admob banner ad.
    private InterstitialAd ia = null; //Admob front ad.
    private bool isTest = true; //Test flag

    protected override void Awake() {
        base.Awake();
        MobileAds.Initialize((initStatus) => {}); //Admob SDK 초기화
    }

    private void Start() {
        LoadInterstitialAd();
    }

    //Loads banner ad.
    public void LoadAd() {
        if (bv == null) CreateBannerView();

        var adRequest = new AdRequest();
        bv.LoadAd(adRequest);
        Debug.Log("Loading banner ad.");
    }

    //Create banner view
    private void CreateBannerView() {
        if (bv != null) DestroyAd();

        bv = new BannerView((isTest ? bannerTestID : bannerID), AdSize.Banner, AdPosition.Bottom);
        Debug.Log("Creating banner view");
    }

    //Destory banner view
    public void DestroyAd() {
        if (bv != null) {
            bv.Destroy();
            bv = null;
            Debug.Log("Destroying banner view");
        }
    }

    //Shows interstitial ad.
    public void ShowInterstitialAd() {
        if (ia != null && ia.CanShowAd()) {
            Debug.Log("Showing interstitial ad.");
            ia.Show();
        }
        else {
            LoadInterstitialAd();
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    //Loads interstitial ad.
    public void LoadInterstitialAd() {
        if (ia != null) {
            ia.Destroy();
            ia = null;
        }

        var adRequest = new AdRequest();
        InterstitialAd.Load((isTest ? frontTestID : frontID), adRequest, (ad, error) => {
            if (error != null || ad == null) {
                Debug.LogError("interstitial ad failed to load an ad with error : " + error);
                return;
            }

            ia = ad;
            RegisterEventHandlers(ia);
            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
        });
    }

    //Interstitial ad. eventhandlers
    private void RegisterEventHandlers(InterstitialAd interstitialAd) {
        //전면 광고가 닫혔을 때
        interstitialAd.OnAdFullScreenContentClosed += () => {
            LoadInterstitialAd();
            Debug.Log("Interstitial ad full screen content closed.");
        };
        //전면 광고를 열지 못했을 때
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) => {
            LoadInterstitialAd();
            Debug.LogError("Interstitial ad failed to open full screen content with error : " + error);
        };
    }

    public override void GameStart() { }

    public override void GameOver() { }
}
