using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Game scene manager
public class GameManager : SceneManager<GameManager> {
    //Player field
    public GameObject player = null; //Player obj.
    public PlayerState ps = null; //Player state
    public PlayerWeapon pw = null; //Player weapon manager

    //Enemy field 
    private List<GameObject> enemy = new List<GameObject>(); //Enemy obj. list
    private Coroutine co = null; //Enemy spawn coroutine
    private WaitForSeconds regenTime = null; //Enemy regen time
    private const int maxEnemyCnt = 1000; //Max enemy count
    private const int spawnRadius = 10; //Enemy spawn radius
    private const float baseRegenTime = 0.5f; //Base regen time
    private int enemyIdx = 1; //Enemy spawn index

    public int EnemyCnt { get; set; } //Enemy count
    public int MaxEnemyCnt { get; }

    //Timer field
    private Text txtTimer = null; //Timer UI
    private float timer = 0f; //Game timer

    //Fever field
    private const float baseFeverRegenTime = 0.25f; //Base fever regen time
    private const float unitRegenTime = 0.05f; //Unit regen time
    private const float minRegenTime = 0.05f; //Min regen time
    private const float baseFeverTime = 20f; //Base fever time
    private const float unitFeverTime = 5f; //Unit fever time
    private const float maxFeverTime = 30f; //Min fever time
    private const float baseFeveringTime = 10f; //Base fevering time
    private const float unitFeveringTime = 5f; //Unit fevering time
    private bool isFever = false; //Fever time frag
    private float feverTimer = 0f; //fever timer
    private float feverTime = 20f; //Enemy spawn fever time
    private float feveringTime = 10f; //Fevering time

    //Settings UI field
    private GameObject settingsWindow = null; //Settings window obj.
    private GameObject btnSettings = null; //Settings window btn.

    //Exp gauge UI field
    private const float screenWidth = 1920f; //Screen width
    private RectTransform expGauge = null; //Exp UI

    //UI field
    private GameObject gachaWindow = null; //Gach window obj.
    private Text txtLevel = null; //Lv UI
    public GameObject virtualJoystick = null; //Virtual joystick UI

    public int GameState { get; set; } //0: Puase, 1: In game, 2: Game over

    protected override void Awake() {
        base.Awake();
        SetScreenTransitionEffect("UI/Fade", "UI");

        player = GameObject.Find("Player");
        ps = player.GetComponent<PlayerState>();
        pw = player.GetComponent<PlayerWeapon>();

        enemy.Add(Resources.Load<GameObject>("Enemies/Cat"));
        enemy.Add(Resources.Load<GameObject>("Enemies/Skeleton"));
        enemy.Add(Resources.Load<GameObject>("Enemies/Flyingeye"));
        enemy.Add(Resources.Load<GameObject>("Enemies/Goblin"));

        GameObject ui = GameObject.Find("UI");

        txtTimer = ui.transform.GetChild(4).GetComponent<Text>();

        settingsWindow = ui.transform.GetChild(6).gameObject;
        btnSettings = ui.transform.GetChild(5).gameObject;

        expGauge = ui.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>();

        txtLevel = ui.transform.GetChild(3).GetComponent<Text>();
        gachaWindow = Resources.Load<GameObject>("UI/GachaWindow");
        virtualJoystick = ui.transform.GetChild(0).gameObject;
    }

    private void Start() {
        GameStart();
    }

    private void Update() {
        if (GameState != 1) return;

        timer += Time.deltaTime;
        txtTimer.text = timer.ToString("n2");

        feverTimer += Time.deltaTime;
        if (feverTimer > feverTime) {
            if (!isFever) {
                float tmpRegenTime = baseFeverRegenTime - (ps.Lv / 5) * unitRegenTime;

                isFever = true;
                if (tmpRegenTime < minRegenTime) tmpRegenTime = minRegenTime;
                SetRegenTime(tmpRegenTime);
            }
        }
        if (feverTimer > feverTime + feveringTime) {
            float tmpFeverTime = baseFeverTime + (ps.Lv / 5) * unitFeverTime;
            float tmpFeveringTime = baseFeveringTime + (ps.Lv / 5) * unitFeveringTime;

            if (tmpFeverTime > maxFeverTime) tmpFeverTime = maxFeverTime;
            SetFeverTime(tmpFeverTime, tmpFeveringTime);
        }
    }

    //Game start
    public override void GameStart() {
        GameState = 1;
        regenTime = new WaitForSeconds(baseRegenTime);
        co = StartCoroutine(SpawnEnemy());
    }

    //Spawn enemy
    private IEnumerator SpawnEnemy() {
        while (GameState == 1) {
            if (EnemyCnt > maxEnemyCnt) continue;

            int idx = Random.Range(0, enemyIdx);
            float angle = Random.Range(0f, 360f);
            Vector3 posSpawn = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * spawnRadius;
            Instantiate(enemy[idx], posSpawn, Quaternion.identity);

            ++EnemyCnt;
            yield return regenTime;
        }
    }

    //Set regen time
    private void SetRegenTime(float time) {
        regenTime = new WaitForSeconds(time);
    }

    //Set fever field
    private void SetFeverTime(float _feverTime, float _feveringTime) {
        feverTimer = 0f;
        feverTime = _feverTime;
        feveringTime = _feveringTime;
        SetRegenTime(baseRegenTime);
        isFever = false;
    }

    //Level up
    public void LvUp() {
        txtLevel.text = ps.Lv.ToString();
        UpdateExpUI(ps.Exp, ps.MaxExp);
        Instantiate(gachaWindow, transform.position, Quaternion.identity).transform.SetParent(GameObject.Find("UI").transform, false);
        virtualJoystick.GetComponent<VirtualJoystick>().enabled = false;
        virtualJoystick.SetActive(false);
    }

    //Update exp UI
    public void UpdateExpUI(float curEXP, float maxEXP) {
        float width = curEXP / maxEXP * screenWidth;
        expGauge.sizeDelta = new Vector2(width, 0f);
    }

    //Open settings window
    public void TouchSettingsWindow() {
        Time.timeScale = 0f;
        settingsWindow.SetActive(true);
        btnSettings.SetActive(false);
    }

    //Game over
    public override void GameOver() {
        GameState = 2;
        StopCoroutine(co);
        co = null;
        virtualJoystick.SetActive(false);
        AuthManager.Inst.AddLeaderboard(timer);
        LoadScene(0);
    }

    public override void LoadScene(int sceneIdx) {
        base.LoadScene(sceneIdx);

        screenTransitionEffect.GetComponent<ScreenTransitionEffect>().sceneIdx = sceneIdx;
        screenTransitionEffect.GetComponent<ScreenTransitionEffect>().EndEffectFirst();
    }
}
