using Management;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using WhiteSurvivor;

//Main scene manager
public class MainManager : SceneManager<MainManager> {
    private AppManager appm = null; //Application manager
    private AuthManager authm = null; //GPGS, Firebase manager
    private AdmobManager admobm = null; //Google admob manager

    public GameObject loginBoard = null;
    public GameObject btnScore = null;
    public Button btnAuth = null;
    public Button btnLogout = null;

    protected override void Awake() {
        base.Awake();
        SetScreenTransitionEffect("UI/Fade", "UI");
    }

    private void Start() {
        Time.timeScale = 1f;
        appm = AppManager.Inst;
        authm = AuthManager.Inst;
        admobm = AdmobManager.Inst;
        if (authm.FUser != null) {
            if (authm.FUser.IsAnonymous) btnScore.SetActive(false);
            loginBoard.SetActive(false);
            admobm.ShowFrontAd();
        }
        admobm.LoadBannerAdmob();
        btnLogout.gameObject.SetActive(false);
    }

    public void TouchAuthentication() {
        if (loginBoard.activeSelf) {
            if (authm.FUser == null) {
                appm.ShowWarningWindow(WarningStatus.DontLogin);
                return;
            }
            loginBoard.SetActive(false);
        }
        else {
            loginBoard.SetActive(true);
            if (authm.FUser == null) btnLogout.gameObject.SetActive(false);
            else btnLogout.gameObject.SetActive(true);
        }
    }

    //Login UI
    //Touch google login button
    public void TouchGoogleLogin() {
        authm.GPGSLogin();
        if (appm.ws != WarningStatus.SignInOnProgress && appm.ws != WarningStatus.LoggingIn) {
            btnScore.SetActive(true);
            loginBoard.SetActive(false);
        }
    }

    //Touch guest login button
    public void TouchGuestLogin() {
        authm.AnonymouslyLogin();
        if (appm.ws != WarningStatus.SignInOnProgress && appm.ws != WarningStatus.LoggingIn) {
            btnScore.SetActive(false);
            loginBoard.SetActive(false);
        }
    }

    //Touch logout button
    public void TouchLogout() {
        authm.Logout();
        btnLogout.gameObject.SetActive(false);
        appm.ShowContextWindow("You have been logged out");
    }

    //Touch start button
    public void TouchStart() {
        admobm.DestroyAd();
        LoadScene(1);
    }
    
    //Main scene UI
    //Touch score button
    public void TouchScore() {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_white_survivor_leaderboard);
    }

    //Touch exit button
    public void TouchExit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public override void GameStart() { }

    public override void GameOver() { }

    public override void LoadScene(int sceneIdx) {
        base.LoadScene(sceneIdx);

        screenTransitionEffect.GetComponent<ScreenTransitionEffect>().sceneIdx = sceneIdx;
        screenTransitionEffect.GetComponent<ScreenTransitionEffect>().EndEffectFirst();
    }
}
