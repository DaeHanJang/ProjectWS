using UnityEngine;

//오브
public class Orb : MonoBehaviour {
    private OrbState os = null;

    private void Start() {
        os = GameManager.Inst.player.GetComponent<OrbState>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = os.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;
    }
}
