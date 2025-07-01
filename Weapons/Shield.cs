using UnityEngine;

//Shield
public class Shield : Weapon {
    private Transform playerPos = null;

    private const float coolTime = 0.5f;
    private float timer = 0f;

    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<ShieldFactory>();
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update() {
        timer += Time.deltaTime;

        if (GameManager.Inst.player) transform.position = playerPos.position;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        if (timer < coolTime) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
        timer = 0f;
    }
}
