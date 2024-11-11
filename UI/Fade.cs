using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour {
    private RectTransform rt = null;
    private Animator at = null;
    private int sceneIdx = 0;

    private void Awake() {
        rt = GetComponent<RectTransform>();
        at = GetComponent<Animator>();
    }

    private void Start() {
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void SetFadeout(int idx) {
        sceneIdx = idx;
        at.SetTrigger("Fadeout");
    }

    public void SetActiveFalse() {
        gameObject.SetActive(false);
    }

    public void SetMoveScene() {
        SceneManager.LoadScene(sceneIdx);
    }
}
