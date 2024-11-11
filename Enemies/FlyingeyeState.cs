using UnityEngine;

public class FlyingeyeState : EnemyState {
    private Animator at = null;
    private AudioSource _as = null;

    protected override void Awake() {
        base.Awake();
        at = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
    }

    protected override void Start() {
        base.Start();
        level -= 19;
        SetIncreaseState();
        SetState();
    }

    protected override void SetIncreaseState() {
        increaseHp = 200f;
        increaseExp = 1.5f;
        increaseStr = 2f;
        increaseDef = 5f;
    }

    protected override void SetState() {
        hp = 1500f;
        exp = 1000000f;
        str = 1500000f;
        def = 20f;
        hp += increaseHp * (level - 1);
        if (level - 1 != 0) {
            exp *= increaseExp * (level - 1);
            str *= increaseStr * (level - 1);
        }
        def += increaseDef * (level - 1);
        speed = 4f;
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
