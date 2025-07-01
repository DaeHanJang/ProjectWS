using Management;
using UnityEngine;
using UnityEngine.UI;
using WhiteSurvivor;

//Application manager
public class AppManager : GameManager<AppManager> {
    private GameObject aw = null; //Alert window prefab
    private GameObject window = null; //Alert window obj.
    private Text content = null; //Alert window text

    public ErrorState es = ErrorState.None; //Error state
    public WarningState ws = WarningState.None; //Warning state

    protected override void Awake() {
        base.Awake();
        Application.targetFrameRate = 60;
        aw = Resources.Load<GameObject>("UI/AlertWindow");
    }

    private void Start() {
        window = Instantiate(aw, new Vector3(0f, 0f), Quaternion.identity);
        content = window.transform.GetChild(0).GetComponent<Text>();
        window.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(TouchExitWindow);
        window.SetActive(false);
    }

    //Show warning log
    public void ShowWarningWindow(WarningState warningStatus) {
        ws = warningStatus;
        content.text = WarningContext();
        window.SetActive(true);
    }

     //Show error log
    public void ShowErrorWindow(ErrorState errorStatus) {
        es = errorStatus;
        content.text = es.ToString();
        window.SetActive(true);
    }

    //Show log
    public void ShowContextWindow(string text) {
        content.text = text;
        window.SetActive(true);
    }

    //Show error context log
    public void ShowContextWindow(ErrorState errorStatus, string text) {
        es = errorStatus;
        content.text = text;
        window.SetActive(true);
    }

    //Touch exit button
    public void TouchExitWindow() {
        if (es != ErrorState.None) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        window.SetActive(false);
    }

    //Mapping warning state to string
    public string WarningContext() {
        switch (ws) {
            case WarningState.SignInOnProgress: return "Login in progress";
            case WarningState.LoggedIn: return "Already logged in";
            case WarningState.GuestLogin: return "The leaderboard is not available for guest login";
            case WarningState.Logout: return "Already logged out";
            case WarningState.NotLogin: return "Need to log in";
            default: return string.Empty;
        }
    }

    public override void GameStart() { }

    public override void GameOver() { }
}
