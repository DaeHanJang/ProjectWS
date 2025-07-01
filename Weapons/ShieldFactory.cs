using System.Collections;
using UnityEngine;

//Shield factory
public class ShieldFactory : WeaponFactory {
    private GameObject shield = null;
    private const float maxScale = 4f;
    private Vector3 scaleIncrease = Vector3.zero;
    private WaitForSeconds delay2 = null;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Shield");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 1f;
        Time = 5f;

        WeaponStrIncrease = 0.25f;
        TimeDecrease = 1f; //Time increase
        scaleIncrease = new Vector3(0.25f, 0.25f, 0.25f);

        MinTime = 10f; //Max time

        Dmg = ps.Str * WeaponStr;

        delay = new WaitForSeconds(Time);
        delay2 = new WaitForSeconds(Time / 2f);

        shield = Instantiate(projectile, transform.position, Quaternion.identity);
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            shield.SetActive(true);
            yield return delay;
            shield.SetActive(false);
            yield return delay2;
        }
    }

    [ContextMenu("LvUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (Time < MinTime) {
            Time += TimeDecrease;
            delay = new WaitForSeconds(Time);
            delay2 = new WaitForSeconds(Time / 2f);
        }
        if (shield.transform.localScale.x < maxScale) shield.transform.localScale += scaleIncrease;
        Dmg = ps.Str * WeaponStr;
    }
}
