using Management;
using UnityEngine;
using UnityEngine.Audio;

//Sound manager
public class SoundManager : SceneManager<SoundManager> {
    public AudioMixer am = null;

    public void SetVolume(string groupName, float value) {
        am.SetFloat(groupName, Mathf.Log10(value) * 20);
    }

    public override void GameStart() {}

    public override void GameOver() {}
}
