using System.Collections;
using UnityEngine;

//�÷��̾� �������ͽ�
public class PlayerState : MonoBehaviour {
    private PlayerWeapon pw = null; //�÷��̾� ���� ���� ������Ʈ
    private Animator at = null;
    private AudioSource _as = null;
    private RectTransform hpGauge = null; //ü�� UI
    private float increaseHp = 2f; //ü�� ������
    private float increaseExp = 2f; //����ġ ������
    private float increaseStr = 2f; //���ݷ� ������
    private float increaseDef = 0.5f; //���� ������
    private float increaseSpeed = 0.2f; //�ӵ� ������
    private float maxHp = 10f; //�ִ� ü��
    private float maxSpeed = 6f; //�ִ� �ӵ�
    private bool bDie = false;

    private float minExp = 0f; //���� ���� �ִ� ����ġ��
    private float maxExp = 10f; //���� ���� �ִ� ����ġ��
    public float hp = 10f; //ü��
    public float exp = 0f; //����ġ
    public float str = 1f; //���ݷ�
    public float def = 0f; //����
    public float speed = 3f; //�ӵ�
    public int level = 1; //����

    private void Awake() {
        pw = GetComponent<PlayerWeapon>();
        at = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
        hpGauge = GameObject.Find("HpGauge").GetComponent<RectTransform>();
    }

    private void Update() {
        //ü�� 0�� ��
        if (hp <= 0f && !bDie) SetDie();

        if (exp >= maxExp) LevelUp();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //���� �ƴ� ���

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = es.str - def; //�� ���ݷ� - �÷��̾� ����
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

    //ü�� ����
    public void UpdateHp(float value) {
        hp += value;
        if (hp > maxHp) hp = maxHp;
        if (hp < 0) hp = 0;
        float width = 0.8f - hp / maxHp * 0.8f;
        hpGauge.sizeDelta = new Vector2(width, 0.2f);
    }

    //����ġ ȹ��
    public void AddExp(float _exp) {
        exp += _exp;
        GameManager.Inst.SetExpGauge(exp - minExp, maxExp - minExp);
    }

    //���� ��
    [ContextMenu("LevelUp")]
    private void LevelUp() {
        ++level;
        minExp = maxExp;
        maxExp *= increaseExp; //����ġ �������� ������ ����
        maxHp *= increaseHp; //ü�� �������� ������ ����
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
        pw.DestoryWeapon(); //���� ���� �ý��� ����
        GameManager.Inst.GameOver(); //���� ����
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
