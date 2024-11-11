using UnityEngine;

public class Wall : MonoBehaviour {
    private WallState ws = null;
    private PlayerMovement pm = null;
    private Vector3 directionVec;
    private float radius = 1f;

    private void Start() {
        ws = GameManager.Inst.player.GetComponent<WallState>();
        pm = GameManager.Inst.pm;
        if (pm.moveVec == Vector2.zero) directionVec = Vector3.left;
        else directionVec = pm.moveVec.normalized;
    }

    private void Update() {
        if (pm.moveVec != Vector2.zero) directionVec = pm.moveVec.normalized;
        float angle = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg;

        transform.position = GameManager.Inst.player.transform.position + directionVec * radius;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = ws.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;
    }
}
