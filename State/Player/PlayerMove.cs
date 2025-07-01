using System.Collections;
using UnityEngine;

//Player move state
public class PlayerMove : State {
    private PlayerState ps = null;
    private SpriteRenderer sr = null;
    private Coroutine co = null;

    private void Awake() {
        owner = GameObject.Find("Player");
        at = owner.GetComponent<Animator>();
        ps = owner.GetComponent<PlayerState>();
        sr = owner.GetComponent<SpriteRenderer>();
    }

    public override void OnStateEnter() {
        at.SetBool("Move", true);
        OnStateUpdate();
    }

    public override void OnStateUpdate() {
        if (co is null) co = StartCoroutine(CoOnStateUpdate());
    }

    private IEnumerator CoOnStateUpdate() {
        while (true) {
            owner.transform.position += ps.inputVec * ps.Speed * Time.deltaTime;
            if (ps.inputVec.x <= 0) sr.flipX = true;
            else sr.flipX = false;

            yield return null;
        }
    }

    public override void OnStateExit() {
        StopCoroutine(co);
        co = null;
        at.SetBool("Move", false);
    }
}
