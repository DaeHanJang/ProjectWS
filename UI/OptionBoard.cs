using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
//�ɼ�â
public class OptionBoard : MonoBehaviour {
    private Button btnOption = null;
    private Text txtState = null;

    private void Awake() {
        btnOption = GameObject.Find("Option").GetComponent<Button>();
        txtState = GameObject.Find("txtState").GetComponent<Text>();
    }

    private void OnEnable() {
        txtState.text = $"Str : {GameManager.Inst.ps.str}\nDef : {GameManager.Inst.ps.def}";
    }

    //����� �����̴� ����
    public void SetSlider(GameObject obj) {
        string sliderName = obj.name;
        float sliderValue = obj.GetComponent<Slider>().value;
        SoundManager.inst.SetVolume(sliderName, sliderValue);
        GameObject.Find($"txt{sliderName}Volume").GetComponent<Text>().text = $"{sliderName}({Mathf.Round(sliderValue * 100)})";
    }

    public void ClickBackButton() {
        gameObject.SetActive(false);
        btnOption.interactable = true;
        Time.timeScale = 1f;
    }

    public void ClickExitButton() {
        GameManager.Inst.LoadScene(0);
    }
}
