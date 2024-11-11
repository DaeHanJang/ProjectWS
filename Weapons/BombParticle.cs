using UnityEngine;

public class BombParticle : MonoBehaviour {
    private BombState bs = null;
    private int cntHit = 0;

    private void Start() {
        bs = GameManager.Inst.player.GetComponent<BombState>();
    }

    private void OnParticleCollision(GameObject other) {
        if (!other.CompareTag("Enemy")) return;
        if (cntHit >= bs.maxCntHit) return; //�ִ� ���� ������ ���� �ߴٸ� ����

        EnemyState es = other.gameObject.GetComponent<EnemyState>();
        float damage = bs.strength - es.def; //���� ���ݷ� - �� ����
        es.hp -= (damage < 0) ? 0 : damage;

        ++cntHit; //���� ���� �� ����
    }
}
