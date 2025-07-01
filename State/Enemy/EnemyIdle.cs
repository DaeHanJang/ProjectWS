using UnityEngine;

//Enemy idle state
public class EnemyIdle : State {
    private void Awake() {
        owner = transform.root.gameObject;
        at = owner.GetComponent<Animator>();
    }

    public override void OnStateEnter() {
        at.SetBool("Move", false);
    }

    public override void OnStateUpdate() { }

    public override void OnStateExit() { }
}
