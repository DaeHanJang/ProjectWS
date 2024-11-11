using UnityEngine;

public class Claw : MonoBehaviour {
    private ClawState cs = null;

    private void Start() {
        cs = GameManager.Inst.player.GetComponent<ClawState>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = cs.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
