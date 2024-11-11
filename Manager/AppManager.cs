using Management;
using UnityEngine;
using UnityEngine.UI;
using WhiteSurvivor;

//Application manager
public class AppManager : GameManager<AppManager> {
    private GameObject confirmationWindow = null;
    private GameObject window = null;

    public ErrorStatus es = ErrorStatus.None;
    public WarningStatus ws = WarningStatus.None;

    protected override void Awake() {
        base.Awake();
        Application.targetFrameRate = 60;
        confirmationWindow = Resources.Load<GameObject>("UI/ConfirmationWindow");
    }

    public void ShowWarningWindow(WarningStatus warningStatus) {
        ws = warningStatus;
        window = Instantiate(confirmationWindow, new Vector3(0f, 0f), Quaternion.identity);
        window.GetComponentInChildren<Text>().text = WarningContext();
        window.GetComponentInChildren<Button>().onClick.AddListener(TouchExitWindow);
    }

    public void ShowErrorWindow(ErrorStatus errorStatus) {
        es = errorStatus;
        window = Instantiate(confirmationWindow, new Vector3(0f, 0f), Quaternion.identity);
        window.GetComponentInChildren<Text>().text = es.ToString();
        window.GetComponentInChildren<Button>().onClick.AddListener(TouchExitWindow);
    }

    public void TouchExitWindow() {
        if (es != ErrorStatus.None) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        Destroy(window);
        window = null;
    }

    public string WarningContext() {
        switch (ws) {
            case WarningStatus.GuestLogin: return "If you login as guest, it will not be recorded on the leaderboard";
            case WarningStatus.SignInOnProgress: return "Login in progress";
            case WarningStatus.LoggingIn: return "You are already logged in";
            case WarningStatus.DontLogin: return "Do not log in";
            default: return "";
        }
    }

    public override void GameStart() { }

    public override void GameOver() { }
}
