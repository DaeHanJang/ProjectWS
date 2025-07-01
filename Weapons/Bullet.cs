using UnityEngine;

//Bullet
public class Bullet : Weapon {
    private Rigidbody2D rb = null;
    public Transform enemyPos = null;
    private bool isDetory = false;
    private bool isPlay = true;

    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<BulletFactory>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Vector2 posAttack = enemyPos.position - transform.position;
        rb.velocity = posAttack.normalized * wf.Speed;

        Invoke("AudioClipDone", GetComponent<AudioSource>().clip.length / 2f);
    }

    private void Update() {
        if (isDetory && !isPlay) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);

        isDetory = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("DetectionRange")) return;

        Destroy(gameObject);
    }

    private void AudioClipDone() {
        isPlay = false;
    }
}
