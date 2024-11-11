using UnityEngine;

//메인 카메라
public class FollowCamera : MonoBehaviour {
    private GameObject player = null;

    private void Start() {
        player = GameManager.Inst.player;
    }

    private void LateUpdate() {
        if (player) {
            transform.position = player.transform.position + new Vector3(0f, 0f, -10f);
        }
    }
}
