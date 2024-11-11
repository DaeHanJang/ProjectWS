using UnityEngine;

public class CameraResolution : MonoBehaviour {
    private void Awake() {
        Camera cam = GetComponent<Camera>();
        Rect rect = cam.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / (16f / 9f);
        float scaleWidht = 1f / scaleHeight;
        if (scaleHeight < 1) {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else {
            rect.width = scaleWidht;
            rect.x = (1f - scaleWidht) / 2f;
        }
        cam.rect = rect;
    }
}
