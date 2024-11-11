using UnityEngine;

//적
public abstract class EnemyState : MonoBehaviour {
    protected EnemyMovement em = null;
    protected SpriteRenderer sr = null;
    protected Rigidbody2D rb = null;
    protected Collider2D col = null;
    protected Color primitiveColor;
    protected float increaseHp = 0f; //체력 증가값
    protected float increaseExp = 0f; //경험치 증가값
    protected float increaseStr = 0f; //공격력 증가값
    protected float increaseDef = 0f; //방어력 증가값
    protected float increaseSpeed = 0f; //속도 증가값
    protected float coolTime = 2f; //공격 대기 시간
    protected int level = 0;
    protected bool bDie = false;

    public bool bAttack = true; //공격 가능 여부
    public float hp = 1f; //체력
    public float exp = 1f; //주는 경험치
    public float str = 1f; //공격력
    public float def = 0f; //방어력
    public float speed = 2f; //속도
    public float coolTimeTimer = 0f; //공격 대기 시간 타이머

    protected virtual void Awake() {
        em = GetComponent<EnemyMovement>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        primitiveColor = sr.color;
    }

    protected virtual void Start() {
        level = GameManager.Inst.ps.level;
    }

    protected virtual void Update() {
        //체력 0일 시
        if (hp <= 0f && !bDie) SetDie();

        //공격 후 공격 대기 시간 타이머 진행
        if (!bAttack) {
            coolTimeTimer += Time.deltaTime;
            if (coolTimeTimer >= coolTime) { //공격 대기 시간 종료
                coolTimeTimer = 0f; //타이머 초기화
                bAttack = true; //공격 가능
            }
        }
    }

    protected virtual void SetDie() {
        --GameManager.Inst.enemyCnt;
        GameManager.Inst.ps.AddExp(exp); //경험치 획득
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor * 0.8f;
    }

    protected virtual void OnCollisionExit2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor * 0.8f;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor;
    }

    protected abstract void SetState(); //적 스테이터스 설정

    protected abstract void SetIncreaseState(); //적 증가량 설정
}
