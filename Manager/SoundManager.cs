using UnityEngine;
using UnityEngine.Audio;

//사운드 매니저
public class SoundManager : MonoBehaviour {
    public static SoundManager inst = null;
    public AudioMixer am = null; //오디오 믹서

    private void Awake() {
        inst = this;
    }

    //오디오 믹서 볼륨 조절
    public void SetVolume(string groupName, float value) {
        am.SetFloat(groupName, Mathf.Log10(value) * 20);
    }
}
