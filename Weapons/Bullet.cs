using UnityEngine;

//�Ѿ�
public class Bullet : MonoBehaviour {
    private BulletState bs = null; //�Ѿ� �������ͽ� ���� ������Ʈ
    private Rigidbody2D rb = null;
    private int cntHit = 0;
    private bool isDetoryReady = false;
    private bool audioDone = false;

    public Transform posEnemy = null;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        bs = GameManager.Inst.player.GetComponent<BulletState>();

        Vector2 posAttack = posEnemy.position - transform.position;
        rb.velocity = posAttack.normalized * bs.speed;

        Invoke("AudioClipDone", GetComponent<AudioSource>().clip.length / 2f);
    }

    private void Update() {
        if (isDetoryReady) {
            if (audioDone) Destroy(gameObject);
            GetComponent<SpriteRenderer>().sprite = null;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //���� �ƴ� ���
        if (cntHit >= bs.maxCntHit) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = bs.strength - es.def; //���� ���ݷ� - �� ����
        es.hp -= (damage < 0) ? 0 : damage;

        ++cntHit; //���� ���� �� ����
        if (cntHit >= bs.maxCntHit) { //�ִ� ���� ������ ���� �ߴٸ� ����
            isDetoryReady = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("DetectionRange")) return;

        isDetoryReady = true; //�ƹ��� ���� �÷��̾� ���� �Ÿ����� ����� ����
    }

    private void AudioClipDone() {
        audioDone = true;
    }
}
