using System.Collections;
using UnityEngine;

//Bullet Factory
public class BulletFactory : WeaponFactory {
    private PlayerDetection pd = null;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Bullet");
        pd = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerDetection>();
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 2f;
        Speed = 4f;
        ProjectileCnt = 1;
        Time = 2f;

        WeaponStrIncrease = 3f;
        SpeedIncrease = 0.5f;
        ProjectileCntIncrease = 1;
        TimeDecrease = 0.25f;

        MaxSpeed = 6f;
        MaxProjectileCnt = 1;
        MinTime = 1f;

        delay = new WaitForSeconds(Time);

        Dmg = ps.Str * WeaponStr;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            if (pd.Detect()) {
                for (int i = 0; i < ProjectileCnt; ++i) {
                    if (!pd.enemy[i]) break;

                    float y = pd.enemy[i].transform.position.y - transform.position.y;
                    float x = pd.enemy[i].transform.position.x - transform.position.x;
                    float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                    GameObject obj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle + 90f));
                    obj.GetComponent<Bullet>().enemyPos = pd.enemy[i].transform;
                }
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
