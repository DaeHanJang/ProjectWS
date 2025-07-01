//Goblin obj.
public class GoblinState : EnemyState {
    protected override void InitStatus() {
        PlayerState ps = GameManager.Inst.ps;
        Lv = ps.Lv - 4;
        DefIncrease = 5f;
        maxDef = 50f;
        coolTime = 2f;

        Exp = ps.MaxExp * 0.05f;
        Hp = ps.Str * 100f * Lv;
        Str = ps.Hp * 0.1f * Lv;
        Def = Lv * DefIncrease;
        DefCoe = 0.01f;
        if (Def > maxDef) Def = maxDef;
        Speed = ps.Speed * 0.75f;
        IsDie = false;
        IsAttack = true;
    }
}
