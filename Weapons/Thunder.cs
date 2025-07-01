using UnityEngine;

//Thunder
public class Thunder : Weapon {
    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<ThunderFactory>();
    }
    private void Start() {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        Invoke("DestroyObj", GetComponent<AudioSource>().clip.length);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
    }

    public void DestoyReady() {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
