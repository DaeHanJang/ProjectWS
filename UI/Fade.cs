using Management;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : ScreenTransitionEffect {
    /*private RectTransform rt = null;

    private void Awake() {
        rt = GetComponent<RectTransform>();
    }

    private void Start() {
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }*/

    public override void StartEffectFirst() { }

    public override void StartEffectLast() {
        gameObject.SetActive(false);
    }

    public override void EndEffectFirst() {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Fadeout");
    }

    public override void EndEffectLast() {
        if (sceneIdx == -1) return;

        SceneManager.LoadScene(sceneIdx);
    }
}
