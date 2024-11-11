using UnityEngine;
using UnityEngine.XR;

//적 움직임
public class EnemyMovement : MonoBehaviour {
    private GameObject player = null;
    private EnemyState es = null; //적 데이터 담당 컴포넌트
    private SpriteRenderer sr = null;
    private Animator at = null;

    public bool knockBack = false;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        at = GetComponent<Animator>();
    }

    private void Start() {
        player = GameManager.Inst.player;
        es = GetComponent<EnemyState>();
    }

    private void Update() {
        if (GameManager.Inst.gameState != 1) {
            at.SetBool("Move", false);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, es.speed * Time.deltaTime);
        if ((player.transform.position - transform.position).x <= 0) sr.flipX = true;
        else sr.flipX = false;
        at.SetBool("Move", true);
    }
}
