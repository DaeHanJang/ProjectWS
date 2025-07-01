using System.Collections;
using UnityEngine;

//Thunder factory
public class ThunderFactory : WeaponFactory {
    private PlayerDetection pd = null;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Thunder");
        pd = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerDetection>();
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 1f;
        ProjectileCnt = 1;
        Time = 5f;

        WeaponStrIncrease = 2f;
        ProjectileCntIncrease = 2;
        TimeDecrease = 0.5f;

        MaxProjectileCnt = 10;
        MinTime = 2f;

        delay = new WaitForSeconds(Time);

        Dmg = ps.Str * WeaponStr;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            if (pd.Detect()) {
                for (int i = 0; i < ProjectileCnt; i++) {
                    if (!pd.enemy[i]) break;

                    Instantiate(projectile, pd.enemy[i].transform.position, Quaternion.identity);
                }
            }
            yield return delay;
        }
    }


    [ContextMenu("LevelUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (ProjectileCnt < MaxProjectileCnt) ProjectileCnt += MaxProjectileCnt;
        if (Time > MinTime) {
            Time -= TimeDecrease;
            delay = new WaitForSeconds(Time);
        }
        Dmg = ps.Str * WeaponStr;
    }
}
