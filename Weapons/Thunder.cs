using UnityEngine;

public class Thunder : MonoBehaviour {
    private ThunderState ts = null;
    private int cntHit = 0;

    private void Start() {
        ts = GameManager.Inst.player.GetComponent<ThunderState>();
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        Invoke("DestroyObj", GetComponent<AudioSource>().clip.length);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return; //적이 아닐 경우
        if (cntHit >= ts.maxCntHit) return;

            EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = ts.strength - es.def; //무기 공격력 - 적 방어력
        es.hp -= (damage < 0) ? 0 : damage;

        ++cntHit;
    }

    public void DestoyReady() {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
