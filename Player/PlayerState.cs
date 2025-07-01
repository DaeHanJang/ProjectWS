using UnityEngine;

//Player state
public class PlayerState : MonoBehaviour, IStatus {
    private State preState = null; //Previous state
    private State curState = null; //Current state
    public State playerDie = null; //Player die state

    public RectTransform hpUI = null; //Hp UI

    public Vector3 inputVec = Vector3.zero; //Input vector

    private const float limitExp = 10000f;
    private const float limitHp = 10000f;
    private const float maxSpeed = 6f;
    private const float maxDef = 50f;

    public int Lv { get; set; }
    public float Exp { get; set; }
    public float MaxExp { get; set; }
    public float Hp { get; set; }
    public float MaxHp { get; set; }
    public float Str { get; set; }
    public float Def { get; set; }
    public float DefCoe { get; set; }
    public float Speed { get; set; }
    public float ExpIncrease { get; set; }
    public float HpIncrease { get; set; }
    public float StrIncrease { get; set; }
    public float DefIncrease { get; set; }
    public float SpeedIncrease { get; set; }
    public bool IsDie { get; set; } //Death frag

    //Initialization status
    public void InitStatus() {
        Lv = 1;
        Exp = 0f;
        MaxExp = 10f;
        Hp = 10f;
        MaxHp = 10f;
        Str = 1f;
        Def = 0f;
        DefCoe = 0.01f;
        Speed = 3f;
        ExpIncrease = 1.5f;
        HpIncrease = 1.5f;
        StrIncrease = 1f;
        DefIncrease = 1f;
        SpeedIncrease = 0.1f;
        IsDie = false;
    }

    private void Awake() {
        InitStatus();
    }

    private void Update() {
        if (Hp <= 0f && !IsDie) {
            SetState(playerDie);
            Action();
        }
        if (Exp >= MaxExp) LvUp();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        if (es.IsAttack) {
            es.IsAttack = false;
            float damage = es.Str - (es.Str * Def * DefCoe);
            if (damage < 0f) damage = 0f;
            UpdateHp(-damage);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        EnemyState es = collision.gameObject.GetComponent<EnemyState>();
        if (es.IsAttack) {
            es.IsAttack = false;
            float damage = es.Str - (es.Str * Def * DefCoe);
            if (damage < 0f) damage = 0f;
            UpdateHp(-damage);
        }
    }

    //Set state
    public void SetState(State state) {
        preState = curState;
        curState = state;
    }

    //State action
    public void Action() {
        if (preState != null) preState.OnStateExit();

        curState.OnStateEnter();
    }

    //Update hp.
    public void UpdateHp(float value) {
        Hp += value;
        if (Hp > MaxHp) Hp = MaxHp;
        if (Hp < 0f) Hp = 0f;
        float width = 0.8f - Hp / MaxHp * 0.8f;
        hpUI.sizeDelta = new Vector2(width, 0.2f);
    }

    //Update exp.
    public void UpdateExp(float value) {
        Exp += value;
        GameManager.Inst.UpdateExpUI(Exp, MaxExp);
    }

    //Lv. up
    [ContextMenu("LvUp")]
    private void LvUp() {
        ++Lv;
        Exp -= MaxExp;
        if (Exp < 0f) Exp = 0f;
        MaxExp *= ExpIncrease;
        if (MaxExp > limitExp) MaxExp = limitExp;
        MaxHp *= HpIncrease;
        if (MaxHp > limitHp) MaxHp = limitHp;
        Hp = MaxHp;
        Str += StrIncrease;
        Def += DefIncrease;
        if (Def > maxDef) Def = maxDef;
        Speed += SpeedIncrease;
        if (Speed > maxSpeed) Speed = maxSpeed;

        UpdateHp(0);
        GameManager.Inst.LvUp();
    }
}
