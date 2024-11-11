using Management;
using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using WhiteSurvivor;

//Game account manager (GPGS, Firebase-based)
public class AuthManager : GameManager<AuthManager> {
    private AppManager am = null;
    private FirebaseApp fApp = null;
    private FirebaseAuth fAuth = null;
    private FirebaseUser fUser = null;
    private bool IsSignInOnProgress = false;

    public FirebaseApp FApp {
        get { return fApp; }
    }
    public FirebaseAuth FAuth {
        get { return fAuth; }
    }
    public FirebaseUser FUser {
        get { return fUser; }
    }

    private void Start() {
        am = AppManager.Inst;

        //Google play games service 환경 초기화 및 활성화
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestIdToken().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        //Firebase 구동 환경 체크
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var result = task.Result;

            if (result != DependencyStatus.Available) {
                am.ShowErrorWindow(ErrorStatus.FirebaseError);
                Debug.LogError(result.ToString());
            }
            else {
                fApp = FirebaseApp.DefaultInstance;
                fAuth = FirebaseAuth.DefaultInstance;
            }
        });
    }

    //GPGS(Google play game service) login
    public void GPGSLogin() {
        if (IsSignInOnProgress) {
            am.ShowWarningWindow(WarningStatus.SignInOnProgress);
            return;
        }
        if (fUser != null) {
            am.ShowWarningWindow(WarningStatus.LoggingIn);
            return;
        }

        IsSignInOnProgress = true;

        Social.localUser.Authenticate((success) => {
            if (success) {
                StartCoroutine(GPGSFirebaseLogin());
                Debug.Log("Login successful");
            }
            else {
                IsSignInOnProgress = false;
                am.ShowErrorWindow(ErrorStatus.GPGSLoginFailed);
                Debug.Log("Login failed");
            }
        });
    }

    //GPGS firebase authentication
    public IEnumerator GPGSFirebaseLogin() {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken())) yield return null;

        string googleIdToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        Credential credential = GoogleAuthProvider.GetCredential(googleIdToken, null);
        fAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            IsSignInOnProgress = false;

            if (task.IsCanceled) {
                am.ShowErrorWindow(ErrorStatus.FirebaseLoginFailed);
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                am.ShowErrorWindow(ErrorStatus.FirebaseLoginFailed);
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            fUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", fUser.DisplayName, fUser.UserId);
        });
    }

    //guest login
    public void AnonymouslyLogin() {
        if (IsSignInOnProgress) {
            am.ShowWarningWindow(WarningStatus.SignInOnProgress);
            return;
        }
        if (fUser != null) {
            am.ShowWarningWindow(WarningStatus.LoggingIn);
            return;
        }

        AppManager.Inst.ShowWarningWindow(WarningStatus.GuestLogin);
        IsSignInOnProgress = true;

        fAuth.SignInAnonymouslyAsync().ContinueWith(task => {
            IsSignInOnProgress = false;

            if (task.IsCanceled) {
                am.ShowErrorWindow(ErrorStatus.FirebaseLoginFailed);
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                am.ShowErrorWindow(ErrorStatus.FirebaseLoginFailed);
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            fUser = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", fUser.DisplayName, fUser.UserId);
        });
    }

    //Account Logout
    public void Logout() {
        fAuth.SignOut();
        if (Social.localUser.authenticated) PlayGamesPlatform.Instance.SignOut();
        fUser = null;
        Debug.LogFormat("Logout successful");
    }

    //Save record to leaderboard
    public void AddLeaderboard(float timer) {
        if (!Social.localUser.authenticated) return;

        PlayGamesPlatform.Instance.ReportScore((long)timer, GPGSIds.leaderboard_white_survivor_leaderboard, (success) => { });
    }

    public override void GameStart() { }

    public override void GameOver() { }
}
