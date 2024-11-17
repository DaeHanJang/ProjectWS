using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//게임 매니저
public class GameManager : SceneManager<GameManager> {
    public GameObject player = null; //플레이어 오브젝트
    public PlayerState ps = null; //플레이어 데이터 담당 컴포넌트
    public PlayerWeapon pw = null; //플레이어 무기 관리 컴포넌트
    public PlayerMovement pm = null; //플레이어 이동 컴포넌트
    public PlayerDetection pd = null; //플레이어 감지 컴포넌트
    public GameObject touchPad = null;
    public int gameState = 0; //게임 상태 0:대기, 1:인 게임, 2: 게임 오버
    public int enemyCnt = 0; //적 숫자

    private GameObject[] enemy = new GameObject[3];
    private GameObject gachaBoard = null;
    private GameObject optionBoard = null;
    private GameObject virtualJoystick = null;
    private Text txtLevel = null;
    private Text txtTimer = null;
    private RectTransform expGauge = null;
    private Button btnOption = null;
    private KeyValuePair<int, int> enemyIdx = new KeyValuePair<int, int>();
    private float feverTiming = 20f;
    private float regenTime = 1f;
    private float timer = 0f; //게임 진행 시간
    private float feverTimer = 0f;
    private int maxEnemyCnt = 1000; //최대 출현 적 숫자

    protected override void Awake() {
        base.Awake();
        SetScreenTransitionEffect("UI/Fade", "UI");

        player = GameObject.Find("Player");
        ps = player.GetComponent<PlayerState>();
        pw = player.GetComponent<PlayerWeapon>();
        pm = player.GetComponent<PlayerMovement>();
        pd = player.GetComponentInChildren<PlayerDetection>();
        enemy[0] = Resources.Load<GameObject>("Enemies/Cat");
        enemy[1] = Resources.Load<GameObject>("Enemies/Skeleton");
        enemy[2] = Resources.Load<GameObject>("Enemies/Flyingeye");
        gachaBoard = Resources.Load<GameObject>("UI/GachaBoard");
        optionBoard = GameObject.Find("OptionBoard");
        virtualJoystick = GameObject.Find("Joystick");
        touchPad = GameObject.Find("TouchPad");
        txtLevel = GameObject.Find("Level").GetComponent<Text>();
        txtTimer = GameObject.Find("Timer").GetComponent<Text>();
        expGauge = GameObject.Find("ExpGauge").GetComponent<RectTransform>();
        btnOption = GameObject.Find("Option").GetComponent<Button>();
        optionBoard.SetActive(false);
    }

    private void Start() {
        GameStart();
    }

    private void Update() {
        if (gameState != 1) return;

        timer += Time.deltaTime;
        feverTimer += Time.deltaTime;

        txtTimer.text = timer.ToString("n2");
        if (feverTimer >= feverTiming) {
            if (1 <= ps.level && ps.level < 5) regenTime = 0.5f;
            else if (5 <= ps.level && ps.level < 10) regenTime = 0.1f;
            else if (10 <= ps.level && ps.level < 15) regenTime = 0.05f;
            Invoke("SetNormalRegen", 5f * (ps.level / 2f));
        }
    }

    //게임 시작
    public override void GameStart() {
        gameState = 1;
        StartCoroutine(posSpawn());
    }

    //게임 종료
    public override void GameOver() {
        gameState = 2;
        StopCoroutine(posSpawn());
        touchPad.SetActive(false);
        AuthManager.Inst.AddLeaderboard(timer);
        LoadScene(0);
    }

    //레벨 UI 조정
    public void SetLevel() {
        txtLevel.text = ps.level.ToString();
    }

    //경험치 UI 조정
    public void SetExpGauge(float curEXP, float maxEXP) {
        float width = curEXP / maxEXP * 1920f;
        expGauge.sizeDelta = new Vector2(width, 0f);
    }

    public void LevelUp() {
        SetLevel();
        Instantiate(gachaBoard, transform.position, Quaternion.identity).transform.SetParent(GameObject.Find("UI").transform, false);
        Time.timeScale = 0;
        virtualJoystick.SetActive(false);
        touchPad.SetActive(false);
        touchPad.GetComponent<VirtualJoystick>().inputVec = Vector2.zero;
        touchPad.GetComponent<VirtualJoystick>().IsInput = false;
    }

    public void ClickOptionButton() {
        optionBoard.SetActive(true);
        btnOption.interactable = false;
        Time.timeScale = 0;
    }

    //적 스폰
    private IEnumerator posSpawn() {
        int idx = 0;
        while (gameState == 1) {
            if (enemyCnt >= maxEnemyCnt) break;

            float angle = Random.Range(0f, 360f);
            Vector3 posSpawn = player.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 10f;
            if (1 <= ps.level && ps.level < 10) enemyIdx = new KeyValuePair<int, int>(0, 1);
            else if (10 <= ps.level && ps.level < 20) enemyIdx = new KeyValuePair<int, int>(0, 2);
            else if (20 <= ps.level && ps.level < 25) enemyIdx = new KeyValuePair<int, int>(0, 3);
            else if (25 <= ps.level && ps.level < 100) enemyIdx = new KeyValuePair<int, int>(1, 3);
            idx = Random.Range(enemyIdx.Key, enemyIdx.Value);
            Instantiate(enemy[idx], posSpawn, Quaternion.identity);
            enemyCnt++;
            yield return new WaitForSeconds(regenTime);
        }
    }

    private void SetNormalRegen() {
        regenTime = 1f;
        feverTimer = 0f;
        feverTiming = 20f;
    }

    public override void LoadScene(int sceneIdx) {
        base.LoadScene(sceneIdx);

        screenTransitionEffect.GetComponent<ScreenTransitionEffect>().sceneIdx = sceneIdx;
        screenTransitionEffect.GetComponent<ScreenTransitionEffect>().EndEffectFirst();
    }
}
