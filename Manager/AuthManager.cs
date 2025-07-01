using Management;
using System;
using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using WhiteSurvivor;

//Authentication manager
public class AuthManager : GameManager<AuthManager> {
    private AppManager am = null; //Application manager
    private FirebaseApp fApp = null; //Firebase SDK class
    private FirebaseAuth fAuth = null; //Firebase authentication class
    private FirebaseUser fUser = null; //Firebase user class
    private bool IsSignInOnProgress = false; //Logging in flag

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

        //GPGS ȯ�� �ʱ�ȭ �� Ȱ��ȭ
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestIdToken().Build(); //GPGS ȯ�� ������
        PlayGamesPlatform.InitializeInstance(config); //GPGS ȯ�� �ʱ�ȭ
        PlayGamesPlatform.DebugLogEnabled = true; //GPGS ����� �α� Ȱ��ȭ
        PlayGamesPlatform.Activate(); //GPGS Ȱ��ȭ

        //Firebase ���� ȯ�� üũ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                fApp = FirebaseApp.DefaultInstance;
                fAuth = FirebaseAuth.DefaultInstance;
            }
            else {
                am.ShowContextWindow(ErrorState.FirebaseInitializationError, String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    //GPGS login
    public void GPGSAuthenticate() {
        if (IsSignInOnProgress) {
            am.ShowWarningWindow(WarningState.SignInOnProgress);
            return;
        }
        if (fUser != null) {
            am.ShowWarningWindow(WarningState.LoggedIn);
            return;
        }

        IsSignInOnProgress = true;

        //GPGS ���� ����
        Social.localUser.Authenticate((success) => {
            if (success) {
                StartCoroutine(FirebaseGPGSLogin());
            }
            else {
                IsSignInOnProgress = false;
                am.ShowErrorWindow(ErrorState.GPGSAuthenticationFailure);
                Debug.Log("GPGS authentication failure");
            }
        });
    }

    //Firebase Google login
    public IEnumerator FirebaseGPGSLogin() {
        string authCode = ((PlayGamesLocalUser)Social.localUser).GetIdToken(); //GPGS ���� ID ��ū

        while (String.IsNullOrEmpty(authCode)) yield return null; //��ȿ�� ID ��ū�� ������ ������ ��ٸ�

        //ID ��ū�� Credential ��ü�� ��ȯ
        //Credential: Firebase ������ ���� �ڰ� ���� ��ü
        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        //Firebase �α��� ��û
        fAuth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                am.ShowErrorWindow(ErrorState.SignInWithCredentialAsyncCanceled);
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                am.ShowContextWindow(ErrorState.SignInWithCredentialAsyncError, task.Exception.ToString());
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            IsSignInOnProgress = false;
            fUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", fUser.DisplayName, fUser.UserId);
        });
    }

    //Firebase guest login
    public void FirebaseAnonymouslyLogin() {
        if (IsSignInOnProgress) {
            am.ShowWarningWindow(WarningState.SignInOnProgress);
            return;
        }
        if (fUser != null) {
            am.ShowWarningWindow(WarningState.LoggedIn);
            return;
        }

        IsSignInOnProgress = true;

        fAuth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                am.ShowErrorWindow(ErrorState.SignInAnonymouslyAsyncCanceled);
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                am.ShowContextWindow(ErrorState.SignInAnonymouslyAsyncError, task.Exception.ToString());
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            IsSignInOnProgress = false;
            fUser = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", fUser.DisplayName, fUser.UserId);
        });
    }

    //Logout
    public void Logout() {
        if (fAuth.CurrentUser == null) {
            am.ShowWarningWindow(WarningState.Logout);
            return;
        }

        fAuth.SignOut();
        if (Social.localUser.authenticated) PlayGamesPlatform.Instance.SignOut();
        fUser = null;

        am.ShowContextWindow("Logout successful");
        Debug.Log("Logout successful");
    }

    //Save GPGS leaderboard records
    public void AddLeaderboard(float timer) {
        if (!Social.localUser.authenticated) return;

        PlayGamesPlatform.Instance.ReportScore((long)timer, GPGSIds.leaderboard_white_survivor_leaderboard, (success) => { });
    }

    public override void GameStart() { }

    public override void GameOver() { }
}
