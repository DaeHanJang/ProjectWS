using UnityEngine;

//Bomb particle
public class BombParticle : Weapon {
    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<BombFactory>();
    }

    private void OnParticleCollision(GameObject other) {
        if (!other.CompareTag("Enemy")) return;

        EnemyState es = other.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
    }
}
