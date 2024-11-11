using UnityEngine;

public class Shiled : MonoBehaviour {
    private ShieldState ss = null;
    private Transform posPlayer = null;
    private float coolTime = 0.2f;
    private float timer = 0f;

    private void Awake() {
        ss = GameManager.Inst.player.GetComponent<ShieldState>();
    }

    private void Start() {
        posPlayer = GameManager.Inst.player.transform;
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer >= coolTime) timer = 0f;

        if (GameManager.Inst.player) transform.position = posPlayer.position;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우
        if (timer != 0f) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = ss.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;
    }
}
