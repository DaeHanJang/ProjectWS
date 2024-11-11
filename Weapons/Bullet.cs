using UnityEngine;

//총알
public class Bullet : MonoBehaviour {
    private BulletState bs = null; //총알 스테이터스 관리 컴포넌트
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
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우
        if (cntHit >= bs.maxCntHit) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = bs.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;

        ++cntHit; //현재 공격 수 증가
        if (cntHit >= bs.maxCntHit) { //최대 공격 수까지 도달 했다면 제거
            isDetoryReady = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("DetectionRange")) return;

        isDetoryReady = true; //아무일 없이 플레이어 감지 거리에서 벗어나면 제거
    }

    private void AudioClipDone() {
        audioDone = true;
    }
}
