using UnityEngine;
using UnityEngine.Audio;

//���� �Ŵ���
public class SoundManager : MonoBehaviour {
    public static SoundManager inst = null;
    public AudioMixer am = null; //����� �ͼ�

    private void Awake() {
        inst = this;
    }

    //����� �ͼ� ���� ����
    public void SetVolume(string groupName, float value) {
        am.SetFloat(groupName, Mathf.Log10(value) * 20);
    }
}
