using System.Collections;
using UnityEngine;

//Wall factory
public class WallFactory : WeaponFactory {
    private GameObject wall = null;
    private const float maxScale = 0.45f;
    private Vector3 scaleIncrease = Vector3.zero;
    private WaitForSeconds delay2 = null;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Wall");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 1f;
        Time = 5f;

        WeaponStrIncrease = 1f;
        TimeDecrease = 1f;
        scaleIncrease = new Vector3(0.05f, 0.05f, 0.05f);

        MinTime = 10f;

        Dmg = ps.Str * WeaponStr;

        delay = new WaitForSeconds(Time);
        delay2 = new WaitForSeconds(Time / 2f);

        wall = Instantiate(projectile, transform.position, Quaternion.identity);
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            wall.SetActive(true);
            yield return delay;
            wall.SetActive(false);
            yield return delay2;
        }
    }

    [ContextMenu("LevelUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (Time < MinTime) {
            Time += TimeDecrease;
            delay = new WaitForSeconds(Time);
            delay2 = new WaitForSeconds(Time / 2f);
        }
        if (wall.transform.localScale.x > maxScale) wall.transform.localScale += scaleIncrease;
        Dmg = ps.Str * WeaponStr;
    }
}
