using UnityEngine;

//Player idle state
public class PlayerIdle : State {
    private void Awake() {
        owner = GameObject.Find("Player");
        at = owner.GetComponent<Animator>();
    }

    public override void OnStateEnter() {
        owner.GetComponent<PlayerState>().inputVec = Vector2.zero;
        at.SetBool("Move", false);
    }

    public override void OnStateUpdate() { }

    public override void OnStateExit() { }
}
