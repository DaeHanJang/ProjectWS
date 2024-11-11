using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GachaBoard : MonoBehaviour {
    private RectTransform rt = null;
    private Animator at = null;
    private PlayerWeapon pw = null;
    private Text txtItem1 = null;
    private Text txtItem2 = null;
    private Text txtItem3 = null;
    private int[] items = null;
    private int weaponIdx;

    private void Awake() {
        rt = GetComponent<RectTransform>();
        at = GetComponent<Animator>();
        rt.anchorMin = new Vector2(0.3f, 0.3f);
        rt.anchorMax = new Vector2(0.7f, 0.7f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        txtItem1 = gameObject.transform.GetChild(0).GetComponentInChildren<Text>();
        txtItem2 = gameObject.transform.GetChild(1).GetComponentInChildren<Text>();
        txtItem3 = gameObject.transform.GetChild(2).GetComponentInChildren<Text>();
    }

    private void Start() {
        pw = GameManager.Inst.player.GetComponent<PlayerWeapon>();

        if (pw.weaponCnt < 5) items = Enumerable.Range(0, pw.GetWeaponeTypeCnt()).ToArray<int>();
        else items = pw.weaponsIdx.ToArray();
        Knuth_Shuffle(items);
        txtItem1.text = pw.GetWeaponName(items[0]);
        txtItem2.text = pw.GetWeaponName(items[1]);
        txtItem3.text = pw.GetWeaponName(items[2]);
    }

    public void ClickButton(int n) {
        weaponIdx = n;
        pw.SetWeapon(items[weaponIdx]);
        at.SetTrigger("End");
    }

    public void ReadyCloseBoard() {
        GameManager.Inst.touchPad.SetActive(true);
    }

    public void CloseBoard() {
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    //ÇÇ¼Å ¿¹ÀÌÃ÷ ¼ÅÇÃ ¾Ë°í¸®Áò
    private void Fisher_Yates_Shuffle<T>(ref T[] array) {
        int n = array.Length;
        T[] newArray = new T[n];

        for (int i = 0; i < n; i++) {
            int last = n - 1 - i;
            int r = UnityEngine.Random.Range(0, last + 1);
            newArray[i] = array[r];
            array[r] = array[last];
        }

        array = newArray;
    }

    //Ä¿´©½º ¼ÅÇÃ ¾Ë°í¸®Áò
    private void Knuth_Shuffle<T>(T[] array) {
        int n = array.Length;
        int last = n - 2;
        
        for (int i = 0; i <= last; i++) {
            int r = UnityEngine.Random.Range(i, n);
            T temp = array[i];
            array[i] = array[r];
            array[r] = temp;
        }
    }
}
