//Flyingeye obj.
public class FlyingeyeState : EnemyState {

    protected override void InitStatus() {
        PlayerState ps = GameManager.Inst.ps;
        coolTime = 1f;

        Exp = ps.MaxExp * 0.02f;
        Hp = 1f;
        Str = ps.Hp * 0.1f;
        Def = 0f;
        DefCoe = 0.01f;
        Speed = 4f;
        IsDie = false;
        IsAttack = true;
    }
}
