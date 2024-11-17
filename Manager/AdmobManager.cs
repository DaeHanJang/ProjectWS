using Management;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

//Google admob manager
public class AdmobManager : GameManager<AdmobManager> {
    //Admob id
    private string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    private string bannerID = "ca-app-pub-2782384029579894/5207437794";
    private string frontTestID = "ca-app-pub-3940256099942544/1033173712";
    private string frontID = "ca-app-pub-2782384029579894/8558954476";
    private string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    private string rewardID = "ca-app-pub-2782384029579894/8274383015";

    private BannerView bv = null;
    private InterstitialAd ia = null;
    private RewardedAd ra = null;
    private bool isTest = false;

    protected override void Awake() {
        base.Awake();
        //Mobile Ad. SDK initialization
        MobileAds.Initialize((initStatus) => {
        });
    }

    private void Start() {
        LoadFrontAdmob();
    }

    //Banner
    //Load Banner Admob
    public void LoadBannerAdmob() {
        if (bv == null) CreateBannerView();

        ListenToAdEvents();

        var adRequest = new AdRequest();
        bv.LoadAd(adRequest);
    }
    //Show Banner Admob
    private void CreateBannerView() {
        if (bv != null) DestroyAd();

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        bv = new BannerView((isTest ? bannerTestID : bannerID), AdSize.Banner, AdPosition.Bottom);
    }
    //Destory Banner Admob
    public void DestroyAd() {
        if (bv != null) {
            bv.Destroy();
            bv = null;
        }
    }
    //Banner Admob Handlers
    private void ListenToAdEvents() {
        // Raised when an ad is loaded into the banner view.
        bv.OnBannerAdLoaded += () => {
            Debug.Log("Banner view loaded an ad with response : " + bv.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bv.OnBannerAdLoadFailed += (LoadAdError error) => {
            Debug.LogError("Banner view failed to load an ad with error : " + error);
        };
        // Raised when the ad is estimated to have earned money.
        bv.OnAdPaid += (AdValue adValue) => {
            Debug.Log(String.Format("Banner view paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bv.OnAdImpressionRecorded += () => {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bv.OnAdClicked += () => {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bv.OnAdFullScreenContentOpened += () => {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bv.OnAdFullScreenContentClosed += () => {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    //Front
    //Show Front Admob
    public void ShowFrontAd() {
        if (ia != null && ia.CanShowAd()) {
            Debug.Log("Showing interstitial ad.");
            ia.Show();
        }
        else {
            Debug.LogError("Interstitial ad is not ready yet.");
            LoadFrontAdmob();
        }
    }
    //Load Front Admob
    public void LoadFrontAdmob() {
        if (ia != null) {
            ia.Destroy();
            ia = null;
        }

        var adRequest = new AdRequest();
        InterstitialAd.Load((isTest ? frontTestID : frontID), adRequest, (ad, error) => {
            if (error != null || ad == null) {
                Debug.LogError("interstitial ad failed to load an ad " + "with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
            ia = ad;
            RegisterEventHandlers(ia);
        });
    }
    //Front Admob Handlers
    private void RegisterEventHandlers(InterstitialAd interstitialAd) {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) => {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () => {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () => {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () => {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () => {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadFrontAdmob();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Interstitial ad failed to open full screen content " + "with error : " + error);
            LoadFrontAdmob();
        };
    }

    //Reward(Don't Use)
    //Show Reward Admob
    public void ShowRewardedAd() {
        const string rewardMsg = "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (ra != null && ra.CanShowAd()) {
            ra.Show((Reward reward) => {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
    //Load Reward Admob
    public void LoadRewardAdmob() {
        if (ra != null) {
            ra.Destroy();
            ra = null;
        }

        var adRequest = new AdRequest();
        RewardedAd.Load((isTest ? rewardTestID : rewardID), adRequest, (RewardedAd ad, LoadAdError error) => {
            if (error != null || ad == null) {
                Debug.LogError("Rewarded ad failed to load an ad " + "with error : " + error);
                return;
            }
        
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            ra = ad;
            RegisterEventHandlers(ra);
        });
    }
    //Reward Admob Handlers
    private void RegisterEventHandlers(RewardedAd ad) {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Rewarded ad full screen content closed.");
            LoadRewardAdmob();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Rewarded ad failed to open full screen content " + "with error : " + error);
            LoadRewardAdmob();
        };
    }

    public override void GameStart() { }

    public override void GameOver() { }
}
