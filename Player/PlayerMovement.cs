using UnityEngine;

//플레이어 움직임
public class PlayerMovement : MonoBehaviour {
    private PlayerState ps = null; //플레이어의 각종 데이터 담당 컴포넌트(speed 등)
    private SpriteRenderer sr = null;
    private Animator at = null;

    public Vector2 moveVec = Vector2.zero;

    private void Awake() {
        ps = GetComponent<PlayerState>();
        sr = GetComponent<SpriteRenderer>();
        at = GetComponent<Animator>();
    }

    public void Move(Vector2 inputVec) {
        transform.position += (Vector3)inputVec * ps.speed * Time.deltaTime;
        moveVec = inputVec;
        if (inputVec.x <= 0) sr.flipX = true;
        else sr.flipX = false;
        at.SetBool("Move", true);
    }

    public void Stop() {
        moveVec = Vector2.zero;
        at.SetBool("Move", false);
    }
}
