using UnityEngine;

//����
public class Orb : MonoBehaviour {
    private OrbState os = null;

    private void Start() {
        os = GameManager.Inst.player.GetComponent<OrbState>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //���� �ƴ� ���

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = os.strength - es.def; //���� ���ݷ� - �� ����
        es.hp -= (damage < 0) ? 0 : damage;
    }
}
