using UnityEngine;

public class BombParticle : MonoBehaviour {
    private BombState bs = null;
    private int cntHit = 0;

    private void Start() {
        bs = GameManager.Inst.player.GetComponent<BombState>();
    }

    private void OnParticleCollision(GameObject other) {
        if (!other.CompareTag("Enemy")) return;
        if (cntHit >= bs.maxCntHit) return; //최대 공격 수까지 도달 했다면 제거

        EnemyState es = other.gameObject.GetComponent<EnemyState>();
        float damage = bs.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;

        ++cntHit; //현재 공격 수 증가
    }
}
