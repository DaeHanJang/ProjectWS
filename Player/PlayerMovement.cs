using UnityEngine;

//�÷��̾� ������
public class PlayerMovement : MonoBehaviour {
    private PlayerState ps = null; //�÷��̾��� ���� ������ ��� ������Ʈ(speed ��)
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
