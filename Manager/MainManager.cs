using Management;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using WhiteSurvivor;

//Main scene manager
public class MainManager : SceneManager<MainManager> {
    private AppManager appm = null; //Application manager
    private AuthManager authm = null; //Authentication manager
    private AdmobManager admobm = null; //Google Admob manager

    public GameObject loginBoard = null; //Authentication window
    public GameObject btnScore = null; //Leaderboard Button obj.
    public Button btnAuth = null; //Authentication window button
    public Button btnLogout = null; //Logout button

    protected override void Awake() {
        base.Awake();
        Time.timeScale = 1f;
        SetScreenTransitionEffect("UI/Fade", "UI");
    }

    private void Start() {
        appm = AppManager.Inst;
        authm = AuthManager.Inst;
        admobm = AdmobManager.Inst;
        admobm.LoadAd(); //배너 광고 노출

        //로그인되어 있을 경우(즉, 게임 씬에서 메인 씬으로 돌아온 경우)
        if (authm.FUser != null) {
            if (authm.FUser.IsAnonymous) btnScore.SetActive(false);
            loginBoard.SetActive(false);
            admobm.ShowInterstitialAd(); //전면 광고 노출
        }
    }

    //Touch authentication button
    public void TouchAuthentication() {
        if (loginBoard.activeSelf) {
            if (authm.FUser == null) {
                appm.ShowWarningWindow(WarningState.NotLogin);
                return;
            }
            else loginBoard.SetActive(false);
        }
        else {
            loginBoard.SetActive(true);

            if (authm.FUser is null) btnLogout.gameObject.SetActive(false);
            else btnLogout.gameObject.SetActive(true);
        }
    }

    //Touch GPGS login button
    public void TouchGoogleLogin() {
        if (authm.FUser != null) {
            appm.ShowWarningWindow(WarningState.LoggedIn);
            return;
        }

        authm.GPGSAuthenticate();
        btnScore.SetActive(true);
        loginBoard.SetActive(false);
    }

    //Touch guest login button
    public void TouchGuestLogin() {
        if (authm.FUser != null) {
            appm.ShowWarningWindow(WarningState.LoggedIn);
            return;
        }

        appm.ShowWarningWindow(WarningState.GuestLogin);
        authm.FirebaseAnonymouslyLogin();
        btnScore.SetActive(false);
        loginBoard.SetActive(false);
    }

    //Touch logout button
    public void TouchLogout() {
        if (authm.FUser is null) {
            appm.ShowWarningWindow(WarningState.Logout);
            return;
        }

        authm.Logout();
        loginBoard.SetActive(true);
        btnLogout.gameObject.SetActive(false);
    }

    //Touch start button
    public void TouchStart() {
        if (authm.FUser is null) {
            appm.ShowWarningWindow(WarningState.NotLogin);
            return;
        }

        admobm.DestroyAd();
        LoadScene(1);
    }
    
    //Touch leaderboard button
    public void TouchScore() {
        if (authm.FUser.IsAnonymous) {
            appm.ShowWarningWindow(WarningState.GuestLogin);
            return;
        }

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
