using UnityEngine;

//Mine
public class Mine : Weapon {
    private BoxCollider2D bc = null;
    private Animator at = null;
    private float timer = 0f;
    private bool isExplosion = false;
    private readonly Vector3 baseScale = new Vector3(0.8f, 0.8f, 0.8f);

    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<MineFactory>();
        bc = GetComponent<BoxCollider2D>();
        at = GetComponent<Animator>();
        bc.enabled = false;
    }

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= ((MineFactory)wf).ExplosionTime && !isExplosion) {
            isExplosion = true;
            Vector3 scale = (((MineFactory)wf).Lv - 1) * ((MineFactory)wf).scaleIncrease;
            if (scale.x > ((MineFactory)wf).maxScale) scale = Vector3.one * ((MineFactory)wf).maxScale;
            transform.localScale = baseScale + scale;
            bc.enabled = true;
            at.SetTrigger("Boom");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        float damage = wf.Dmg - (wf.Dmg * es.Def * es.DefCoe);
        if (damage < 0f) damage = 0f;
        es.UpdateHp(damage);
    }

    public void DestroyObj() {
        Destroy(gameObject);
    }
}
