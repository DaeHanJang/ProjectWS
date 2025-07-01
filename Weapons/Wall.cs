using UnityEngine;

//Wall
public class Wall : Weapon {
    private PlayerState ps = null;
    private Vector3 directionVec = Vector3.left;
    private const float radius = 0.5f;

    private const float coolTime = 0.5f;
    private float timer = 0f;

    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<WallFactory>();
        ps = GameObject.Find("Player").GetComponent<PlayerState>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if (ps.inputVec != Vector3.zero) directionVec = ps.inputVec.normalized;
        float angle = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg;

        transform.position = GameManager.Inst.player.transform.position + directionVec * radius;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        if (timer < coolTime) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
        timer = 0f;
    }
}
