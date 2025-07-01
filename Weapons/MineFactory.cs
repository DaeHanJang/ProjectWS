using System.Collections;
using UnityEngine;

//Mine factory
public class MineFactory : WeaponFactory {
    public float maxScale = 2f;
    public Vector3 scaleIncrease = Vector3.zero;

    public float ExplosionTime { get; set; }
    public float ExplosionTimeDecrease { get; set; }
    public float MinExplosionTime { get; set; }

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Mine");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 3f;
        Time = 3f;
        ExplosionTime = 2f;

        WeaponStrIncrease = 3f;
        TimeDecrease = 0.5f;
        scaleIncrease = new Vector3(0.25f, 0.25f, 0.25f);
        ExplosionTimeDecrease = 0.25f;

        MinTime = 1f;
        MinExplosionTime = 0.5f;

        delay = new WaitForSeconds(Time);
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            Instantiate(projectile, transform.position, Quaternion.identity);
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
        if (ExplosionTime > MinExplosionTime) ExplosionTime -= ExplosionTimeDecrease;
        Dmg = ps.Str * WeaponStr;
    }
}
