using System.Collections;
using UnityEngine;

//플레이어 스테이터스
public class PlayerState : MonoBehaviour {
    private PlayerWeapon pw = null; //플레이어 무기 관리 컴포넌트
    private Animator at = null;
    private AudioSource _as = null;
    private RectTransform hpGauge = null; //체력 UI
    private float increaseHp = 2f; //체력 증가값
    private float increaseExp = 2f; //경험치 증가값
    private float increaseStr = 2f; //공격력 증가값
    private float increaseDef = 0.5f; //방어력 증가값
    private float increaseSpeed = 0.2f; //속도 증가값
    private float maxHp = 10f; //최대 체력
    private float maxSpeed = 6f; //최대 속도
    private bool bDie = false;

    private float minExp = 0f; //이전 레벨 최대 경험치량
    private float maxExp = 10f; //현재 레벨 최대 경험치량
    public float hp = 10f; //체력
    public float exp = 0f; //경험치
    public float str = 1f; //공격력
    public float def = 0f; //방어력
    public float speed = 3f; //속도
    public int level = 1; //레벨

    private void Awake() {
        pw = GetComponent<PlayerWeapon>();
        at = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
        hpGauge = GameObject.Find("HpGauge").GetComponent<RectTransform>();
    }

    private void Update() {
        //체력 0일 시
        if (hp <= 0f && !bDie) SetDie();

        if (exp >= maxExp) LevelUp();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = es.str - def; //적 공격력 - 플레이어 방어력
        if (damage < 0) damage = 0;
        UpdateHp(-damage);
        es.bAttack = false;
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        if (es.bAttack) {
            es.bAttack = false;
            float damage = es.str - def;
            if (damage < 0) damage = 0;
            UpdateHp(-damage);
        }
    }

    //체력 갱신
    public void UpdateHp(float value) {
        hp += value;
        if (hp > maxHp) hp = maxHp;
        if (hp < 0) hp = 0;
        float width = 0.8f - hp / maxHp * 0.8f;
        hpGauge.sizeDelta = new Vector2(width, 0.2f);
    }

    //경험치 획득
    public void AddExp(float _exp) {
        exp += _exp;
        GameManager.Inst.SetExpGauge(exp - minExp, maxExp - minExp);
    }

    //레벨 업
    [ContextMenu("LevelUp")]
    private void LevelUp() {
        ++level;
        minExp = maxExp;
        maxExp *= increaseExp; //경험치 증가량의 곱으로 증가
        maxHp *= increaseHp; //체력 증가량의 곱으로 증가
        hp = maxHp;
        str += increaseStr;
        def += increaseDef;
        if (level % 3 == 0 && speed < maxSpeed) speed += increaseSpeed;
        UpdateHp(0);
        GameManager.Inst.SetExpGauge(exp - minExp, maxExp - minExp);
        GameManager.Inst.LevelUp();
    }

    private void SetDie() {
        bDie = true;
        GetComponent<PlayerMovement>().enabled = false;
        pw.DestoryWeapon(); //무기 관리 시스템 제거
        GameManager.Inst.GameOver(); //게임 오버
        at.SetTrigger("Die");
        _as.Play();
        Invoke("DestroyObj", _as.clip.length);
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }

    [ContextMenu("LevelUp10")]
    private void LevelUP10() {
        for (int i = 0; i < 10; i++) LevelUp();
    }
}
