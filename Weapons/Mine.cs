using UnityEngine;

public class Mine : MonoBehaviour {
    private MineState ms = null;
    private BoxCollider2D bc = null;
    private Animator at = null;
    private float timer = 0f;

    private void Awake() {
        bc = GetComponent<BoxCollider2D>();
        at = GetComponent<Animator>();
        bc.enabled = false;
    }

    private void Start() {
        ms = GameManager.Inst.player.GetComponent<MineState>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= ms.timer) {
            transform.localScale = ms.BoomScale;
            bc.enabled = true;
            at.SetTrigger("Boom");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = ms.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
