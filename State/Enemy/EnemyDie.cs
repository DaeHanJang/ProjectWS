using UnityEngine;

//Enemy die state
public class EnemyDie : State {
    private EnemyState es = null;
    private Rigidbody2D rb = null;
    private Collider2D cd = null;
    private AudioSource ads = null;

    private void Awake() {
        owner = transform.root.gameObject;
        at = owner.GetComponent<Animator>();
        es = owner.GetComponent<EnemyState>();
        rb = owner.GetComponent<Rigidbody2D>();
        cd = owner.GetComponent<Collider2D>();
        ads = owner.GetComponent<AudioSource>();
    }

    public override void OnStateEnter() {
        es.IsDie = true;
        rb.isKinematic = true;
        cd.enabled = false;
        --GameManager.Inst.EnemyCnt;
        GameManager.Inst.ps.UpdateExp(es.Exp);
        at.SetTrigger("Die");
        ads.Play();
        Invoke("DestoryOwner", ads.clip.length);
    }

    public override void OnStateExit() { }

    public override void OnStateUpdate() { }

    private void DestoryOwner() {
        Destroy(owner);
    }
}
