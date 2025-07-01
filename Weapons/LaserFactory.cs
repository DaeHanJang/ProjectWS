using System.Collections;
using UnityEngine;

//Laser factory
public class LaserFactory : WeaponFactory {
    private PlayerDetection pd = null;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Laser");
        pd = GameObject.Find("Player").transform.GetChild(1).GetComponent<PlayerDetection>();
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 1f;
        Time = 4f;

        WeaponStrIncrease = 2f;
        TimeDecrease = 0.25f;

        MinTime = 1f;

        Dmg = ps.Str * WeaponStr;

        delay = new WaitForSeconds(Time);
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            if (pd.Detect()) {
                float y = pd.nearestEnemy.transform.position.y - transform.position.y;
                float x = pd.nearestEnemy.transform.position.x - transform.position.x;
                float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle - 90f));
            }

            yield return delay;
        }
    }

    [ContextMenu("LevelUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (Time > MinTime) {
            Time -= TimeDecrease;
            delay = new WaitForSeconds(Time);
        }
        Dmg = ps.Str * WeaponStr;
    }
}
