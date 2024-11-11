using UnityEngine;

public class CatState : EnemyState {
    private Animator at = null;
    private AudioSource _as = null;

    public Sprite[] sprites = new Sprite[6];
    public RuntimeAnimatorController[] ac = new RuntimeAnimatorController[6];

    protected override void Awake() {
        base.Awake();
        at = GetComponent<Animator>();
        _as = GetComponent<AudioSource>();
        int idx = Random.Range(0, 6);
        sr.sprite = sprites[idx];
        at.runtimeAnimatorController = ac[idx];
    }

    protected override void Start() {
        base.Start();
        SetIncreaseState();
        SetState();
    }

    protected override void SetIncreaseState() {
        increaseHp = 5f;
        increaseExp = 1.5f;
        increaseStr = 1.5f;
        increaseDef = 0.5f;
    }

    protected override void SetState() {
        hp = 1f;
        exp = 1f;
        str = 1f;
        def = 0f;
        hp += increaseHp * (level - 1);
        if (level - 1 != 0) {
            exp *= increaseExp * (level - 1);
            str *= increaseStr * (level - 1);
        }
        def += increaseDef * (level - 1);
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
