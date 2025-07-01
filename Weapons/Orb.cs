using UnityEngine;

//Orb
public class Orb : Weapon {
    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<OrbFactory>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
    }
}
