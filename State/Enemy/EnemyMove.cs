using System.Collections;
using UnityEngine;

//Enemy move state
public class EnemyMove : State {
    private EnemyState es = null;
    private SpriteRenderer sr = null;
    private Coroutine co = null;
    private GameObject player = null;

    private void Awake() {
        owner = transform.root.gameObject;
        at = owner.GetComponent<Animator>();
        es = owner.GetComponent<EnemyState>();
        sr = owner.GetComponent<SpriteRenderer>();
    }

    public override void OnStateEnter() {
        player = GameManager.Inst.player;
        at.SetBool("Move", true);
        OnStateUpdate();
    }

    public override void OnStateUpdate() {
        if (co is null) co = StartCoroutine(CoOnStateUpdate());
    }

    private IEnumerator CoOnStateUpdate() {
        while (true) {
            Vector3 inputVec = player.transform.position - owner.transform.position;
            owner.transform.position += inputVec.normalized * es.Speed * Time.deltaTime;
            if (inputVec.normalized.x <= 0) sr.flipX = true;
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
