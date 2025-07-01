using System.Collections;
using UnityEngine;

//Bomb factory
public class BombFactory : WeaponFactory {
    private const float radius = 3f;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Bomb");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 2f;
        Speed = 4f;
        ProjectileCnt = 1;
        Time = 5f;

        WeaponStrIncrease = 3f;
        SpeedIncrease = 0.5f;
        ProjectileCntIncrease = 1;
        TimeDecrease = 0.5f;

        MaxSpeed = 6f;
        MaxProjectileCnt = 4;
        MinTime = 2f;

        delay = new WaitForSeconds(Time);

        Dmg = ps.Str * WeaponStr;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            for (int i = 0; i < ProjectileCnt; i++) {
                float angle = Random.Range(0, 360);
                float radian = angle * Mathf.Deg2Rad;
                float x = Mathf.Cos(radian) * radius;
                float y = Mathf.Sin(radian) * radius;
                GameObject obj = Instantiate(projectile, transform.position, Quaternion.identity);
                obj.GetComponent<Bomb>().destinationPos = transform.position + new Vector3(x, y);
            }
            yield return delay;
        }
    }

    [ContextMenu("LevelUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (Speed < MaxSpeed) Speed += SpeedIncrease;
        if (ProjectileCnt < MaxProjectileCnt) ProjectileCnt += ProjectileCntIncrease;
        if (Time > MinTime) {
            Time -= TimeDecrease;
            delay = new WaitForSeconds(Time);
        }
        Dmg = ps.Str * WeaponStr;
    }
}
