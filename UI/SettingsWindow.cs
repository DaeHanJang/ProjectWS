using UnityEngine;
using UnityEngine.UI;
 
//Settings window
public class SettingsWindow : MonoBehaviour {
    private Button btnSettings = null;
    private Text txtState = null;

    private void Awake() {
        btnSettings = GameObject.Find("BtnSettings").GetComponent<Button>();
        txtState = GameObject.Find("txtState").GetComponent<Text>();
    }

    private void OnEnable() {
        txtState.text = $"Str : {GameManager.Inst.ps.Str}\nDef : {GameManager.Inst.ps.Def}";
    }

    //오디오 슬라이더 조절
    public void SetSlider(GameObject obj) {
        string sliderName = obj.name;
        float sliderValue = obj.GetComponent<Slider>().value;
        SoundManager.Inst.SetVolume(sliderName, sliderValue);
        GameObject.Find($"txt{sliderName}Volume").GetComponent<Text>().text = $"{sliderName}({Mathf.Round(sliderValue * 100)})";
    }

    public void ClickBackButton() {
        gameObject.SetActive(false);
        btnSettings.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ClickExitButton() {
        GameManager.Inst.LoadScene(0);
    }
}
