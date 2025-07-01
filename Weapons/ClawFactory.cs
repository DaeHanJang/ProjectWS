using System.Collections;
using UnityEngine;

//Claw factory
public class ClawFactory : WeaponFactory {
    public const float maxScale = 0.6f;
    public Vector3 scaleIncrease = Vector3.zero;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Claw");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 3f;
        ProjectileCnt = 1;
        Time = 3f;

        WeaponStrIncrease = 3f;
        ProjectileCntIncrease = 1;
        TimeDecrease = 0.25f;
        scaleIncrease = new Vector3(0.05f, 0.05f, 0.05f);

        MaxProjectileCnt = 5;
        MinTime = 1.5f;

        Dmg = ps.Str * WeaponStr;

        delay = new WaitForSeconds(Time);
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            Vector3 directionVec = Vector3.left;
            if (ps.inputVec.x > 0) directionVec = Vector3.right;

            for (int i = 0; i < ProjectileCnt; i++) {
                GameObject obj = Instantiate(projectile, transform.position + directionVec, Quaternion.identity);
                obj.transform.localScale += (Lv - 1) * scaleIncrease;
                if (obj.transform.localScale.x > maxScale) obj.transform.localScale = Vector3.one * maxScale;
                if (directionVec == Vector3.left) obj.GetComponent<SpriteRenderer>().flipX = true;
                directionVec *= -1;
                yield return new WaitUntil(() => obj == null);
            }
            yield return delay;
        }
    }

    [ContextMenu("LevelUp")]
    public override void LvUp() {
        ++Lv;
        WeaponStr += WeaponStrIncrease;
        if (ProjectileCnt < MaxProjectileCnt) ProjectileCnt += ProjectileCntIncrease;
        if (Time > MinTime) {
            Time -= TimeDecrease;
            delay = new WaitForSeconds(Time);
        }
        Dmg = ps.Str * WeaponStr;
    }
}
