using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Orb factory
public class OrbFactory : WeaponFactory {
    private const float radius = 1.5f;
    private List<GameObject> orb = new List<GameObject>();

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Orb");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 1f;
        Speed = 0.5f;
        ProjectileCnt = 1;

        WeaponStrIncrease = 1f;
        ProjectileCntIncrease = 1;

        MaxProjectileCnt = 6;

        Dmg = ps.Str * WeaponStr;

        orb.Add(Instantiate(projectile, transform.position, Quaternion.identity));
    }

    protected override IEnumerator SetWeapon() {
        float angle = 0f;
        while (GameManager.Inst.GameState == 1) {
            if (UnityEngine.Time.timeScale != 0) {
                angle += Speed;
                if (angle >= 360f) angle %= 360f;
                for (int i = 0; i < ProjectileCnt; i++) {
                    float radian = (angle + (i * (360f / ProjectileCnt))) * Mathf.Deg2Rad;
                    float x = Mathf.Cos(radian) * radius;
                    float y = Mathf.Sin(radian) * radius;
                    orb[i].transform.position = transform.position + new Vector3(x, y);
                }
            }

            yield return null;
        }
    }

    [ContextMenu("LvUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (ProjectileCnt < MaxProjectileCnt) {
            ProjectileCnt += ProjectileCntIncrease;
            orb.Add(Instantiate(projectile, transform.position, Quaternion.identity));
        }
        Dmg = ps.Str * WeaponStr;
    }
}
