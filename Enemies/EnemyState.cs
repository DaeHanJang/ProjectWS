using UnityEngine;

//Enemy state
public abstract class EnemyState : MonoBehaviour, IStatus {
    protected SpriteRenderer sr = null; //Sprite comp.
    protected Collider2D col = null; //Collider comp.
    protected State preState = null; //Previous State
    protected State curState = null; //Current State

    public State enemyIdle = null; //Enemy idle state
    public State enemyMove = null; //Enemy move state
    public State enemyDie = null; //Enemy die state

    protected float maxDef;
    protected float coolTime; //Attack cooltime
    protected Color primitiveColor = Color.clear; //Base color
    protected float timer = 0f;
    
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
    public bool IsDie { get; set; } //죽음 여부
    public bool IsAttack { get; set; } //공격 여부
    
    protected virtual void Awake() {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        primitiveColor = sr.color;
    }

    protected virtual void Start() {
        InitStatus();
        SetState(enemyMove);
        Action();
    }

    protected virtual void Update() {
        if (Hp <= 0f && !IsDie) {
            SetState(enemyDie);
            Action();
        }

        if (GameManager.Inst.GameState == 2) {
            SetState(enemyIdle);
            Action();
        }

        //공격 대기 시간
        if (!IsAttack) {
            timer += Time.deltaTime;
            if (timer >= coolTime) {
                timer = 0f;
                IsAttack = true;
            }
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
        Hp -= value;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor * 0.8f;
    }

    protected virtual void OnCollisionExit2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;

        sr.color = primitiveColor * 0.8f;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Weapon")) return;
        
        sr.color = primitiveColor;
    }

    //Initialization status
    protected abstract void InitStatus();
}
