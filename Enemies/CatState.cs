using UnityEngine;

//Cat obj.
public class CatState : EnemyState {
    private Animator at = null;

    public Sprite[] sprites = new Sprite[6];
    public RuntimeAnimatorController[] ac = new RuntimeAnimatorController[6];

    protected override void Awake() {
        base.Awake();
        at = GetComponent<Animator>();
        int idx = Random.Range(0, 6);
        sr.sprite = sprites[idx];
        at.runtimeAnimatorController = ac[idx];
    }

    protected override void InitStatus() {
        PlayerState ps = GameManager.Inst.ps;
        Lv = ps.Lv;
        HpIncrease = 1f;
        maxDef = 10f;
        coolTime = 1.5f;

        Exp = Lv;
        if (Exp < ps.MaxExp * 0.01f) Exp = ps.MaxExp * 0.01f;
        Hp = Lv * HpIncrease;
        if (Hp < ps.Str * 2) Hp = ps.Str * 2;
        Str = Lv;
        if (Str < ps.Hp * 0.01f) Str = ps.Hp * 0.01f;
        Def = Lv - 1;
        if (Def > maxDef) Def = maxDef;
        DefCoe = 0.01f;
        Speed = 2.5f;
        IsDie = false;
        IsAttack = true;
    }
}
