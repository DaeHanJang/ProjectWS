using UnityEngine;

public class SkeletonState : EnemyState {
    private Animator at = null;
    private AudioSource _as = null;

    protected override void Awake() {
        base.Awake();
        at = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
    }

    protected override void Start() {
        base.Start();
        level -= 9;
        SetIncreaseState();
        SetState();
    }

    protected override void SetIncreaseState() {
        increaseHp = 150f;
        increaseExp = 1.5f;
        increaseStr = 1.5f;
        increaseDef = 5f;
    }

    protected override void SetState() {
        hp = 350f;
        exp = 1000f;
        str = 1000f;
        def = 20f;
        hp += increaseHp * (level - 1);
        if (level - 1 != 0) {
            exp *= increaseExp * (level - 1);
            str *= increaseStr * (level - 1);
        }
        def += increaseDef * (level - 1);
        speed = 2f;
        coolTime = 2f;
    }

    protected override void SetDie() {
        bDie = true;
        em.enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        col.enabled = false;
        at.SetTrigger("Die");
        _as.Play();
        --GameManager.Inst.enemyCnt;
        GameManager.Inst.ps.AddExp(exp); //°æÇèÄ¡ È¹µæ
        Invoke("DestroyObj", _as.clip.length);
    }

    public void DestroyObj() { Destroy(gameObject); }
}
