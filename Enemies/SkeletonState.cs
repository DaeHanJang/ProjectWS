//Skeleton obj.
public class SkeletonState : EnemyState {
    protected override void InitStatus() {
        PlayerState ps = GameManager.Inst.ps;
        Lv = ps.Lv - 9;
        HpIncrease = 1.5f;
        StrIncrease = 1.5f;
        DefIncrease = 1.5f;
        maxDef = 70f;
        coolTime = 2f;

        Exp = ps.MaxExp * 0.02f;
        Hp = ps.Str * Lv * HpIncrease;
        Str = ps.Hp * 0.01f * Lv * StrIncrease;
        Def = 10f + Lv * DefIncrease;
        if (Def > maxDef) Def = maxDef;
        DefCoe = 0.01f;
        Speed = 2f;
        IsDie = false;
        IsAttack = true;
    }
}
