using UnityEngine;

public class Wind : Weapon {
    private Rigidbody2D rb = null;

    public Vector3 inputVec;

    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<WindFactory>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Vector2 posAttack = inputVec;
        rb.velocity = posAttack.normalized * wf.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("DetectionRange")) return;

        Destroy(gameObject);
    }
}
