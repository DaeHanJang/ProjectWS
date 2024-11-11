using UnityEngine;

public class Laser : MonoBehaviour {
    private LaserState ls = null;

    public void Start() {
        ls = GameManager.Inst.player.GetComponent<LaserState>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //���� �ƴ� ���

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = ls.strength - es.def; //���� ���ݷ� - �� ����
        es.hp -= (damage < 0) ? 0 : damage;
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
