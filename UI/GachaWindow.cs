using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//Gacha window
public class GachaWindow : MonoBehaviour {
    //Gacha window UI
    private RectTransform rt = null;
    private Animator at = null;

    //Player weapon manager
    private PlayerWeapon pw = null;

    //Text UI
    private Text txtItem1 = null;
    private Text txtItem2 = null;
    private Text txtItem3 = null;

    private int[] weapon = null;

    private void Awake() {
        rt = GetComponent<RectTransform>();
        at = GetComponent<Animator>();

        pw = GameObject.Find("Player").GetComponent<PlayerWeapon>();

        txtItem1 = gameObject.transform.GetChild(0).GetComponentInChildren<Text>();
        txtItem2 = gameObject.transform.GetChild(1).GetComponentInChildren<Text>();
        txtItem3 = gameObject.transform.GetChild(2).GetComponentInChildren<Text>();

        rt.anchorMin = new Vector2(0.3f, 0.3f);
        rt.anchorMax = new Vector2(0.7f, 0.7f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private void Start() {
        Time.timeScale = 0;
        at.enabled = true;
        if (pw.weaponCnt < 5) weapon = Enumerable.Range(0, pw.GetEWeaponMax()).ToArray();
        else weapon = pw.weaponIdx.ToArray();
        Knuth_Shuffle(weapon);
        txtItem1.text = pw.GetWeaponName(weapon[0]);
        txtItem2.text = pw.GetWeaponName(weapon[1]);
        txtItem3.text = pw.GetWeaponName(weapon[2]);
    }

    public void TouchButton(int n) {
        pw.SetWeapon(weapon[n]);
        at.SetTrigger("Close");
    }

    public void CloseWindow() {
        Time.timeScale = 1f;
        GameManager.Inst.virtualJoystick.GetComponent<VirtualJoystick>().enabled = true;
        GameManager.Inst.virtualJoystick.SetActive(true);
        at.enabled = false;
        gameObject.SetActive(false);
    }

    //커누스 셔플 알고리즘
    private void Knuth_Shuffle<T>(T[] array) {
        int n = array.Length;

        for (int i = n - 1; i > 0; --i) {
            int r = Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[r];
            array[r] = temp;
        }
    }
}
