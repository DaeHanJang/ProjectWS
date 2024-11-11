using UnityEngine;

public class Wind : MonoBehaviour {
    private WindState ws = null;
    private Rigidbody2D rb = null;

    public Vector3 inputVec;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        ws = GameManager.Inst.player.GetComponent<WindState>();

        Vector2 posAttack = inputVec;
        rb.velocity = posAttack.normalized * ws.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = ws.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("DetectionRange")) return;

        Destroy(gameObject);
    }
}
