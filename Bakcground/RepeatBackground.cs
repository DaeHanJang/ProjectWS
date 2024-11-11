using UnityEngine;

public class RepeatBackground : MonoBehaviour {
    private BoxCollider2D bc = null;
    private PlayerMovement pm = null;

    private void Awake() {
        bc = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        pm = GameManager.Inst.player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (GameManager.Inst.gameState != 1) return;
        if (!collision.CompareTag("Sight")) return;

        Vector3 posPlayer = GameManager.Inst.player.transform.position;
        Vector3 posMy = transform.position;

        float diffX = posPlayer.x - posMy.x;
        float diffY = posPlayer.y - posMy.y;
        float absDiffX = Mathf.Abs(diffX);
        float absDiffY = Mathf.Abs(diffY);

        if (absDiffX > absDiffY) {
            if (diffX > 0) transform.Translate(Vector3.right * transform.localScale.x * bc.size.x * 2);
            else if (diffX < 0) transform.Translate(Vector3.left * transform.localScale.x * bc.size.x * 2);
        }
        else if (absDiffX < absDiffY) {
            if (diffY > 0) transform.Translate(Vector3.up * transform.localScale.y * bc.size.y * 2);
            else if (diffY < 0) transform.Translate(Vector3.down * transform.localScale.y * bc.size.y * 2);
        }
    }
}
