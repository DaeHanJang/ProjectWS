using UnityEngine;

//��
public abstract class EnemyState : MonoBehaviour {
    protected EnemyMovement em = null;
    protected SpriteRenderer sr = null;
    protected Rigidbody2D rb = null;
    protected Collider2D col = null;
    protected Color primitiveColor;
    protected float increaseHp = 0f; //ü�� ������
    protected float increaseExp = 0f; //����ġ ������
    protected float increaseStr = 0f; //���ݷ� ������
    protected float increaseDef = 0f; //���� ������
    protected float increaseSpeed = 0f; //�ӵ� ������
    protected float coolTime = 2f; //���� ��� �ð�
    protected int level = 0;
    protected bool bDie = false;

    public bool bAttack = true; //���� ���� ����
    public float hp = 1f; //ü��
    public float exp = 1f; //�ִ� ����ġ
    public float str = 1f; //���ݷ�
    public float def = 0f; //����
    public float speed = 2f; //�ӵ�
    public float coolTimeTimer = 0f; //���� ��� �ð� Ÿ�̸�

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
        //ü�� 0�� ��
        if (hp <= 0f && !bDie) SetDie();

        //���� �� ���� ��� �ð� Ÿ�̸� ����
        if (!bAttack) {
            coolTimeTimer += Time.deltaTime;
            if (coolTimeTimer >= coolTime) { //���� ��� �ð� ����
                coolTimeTimer = 0f; //Ÿ�̸� �ʱ�ȭ
                bAttack = true; //���� ����
            }
        }
    }

    protected virtual void SetDie() {
        --GameManager.Inst.enemyCnt;
        GameManager.Inst.ps.AddExp(exp); //����ġ ȹ��
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

    protected abstract void SetState(); //�� �������ͽ� ����

    protected abstract void SetIncreaseState(); //�� ������ ����
}
