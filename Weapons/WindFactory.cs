using System.Collections;
using UnityEngine;

//Wind factory
public class WindFactory : WeaponFactory {
    private const float maxScale = 0.75f;
    private Vector3 scaleIncrease = Vector3.zero;

    protected override void Awake() {
        base.Awake();
        projectile = Resources.Load<GameObject>("Weapons/Wind");
    }

    protected override void InitState() {
        Lv = 1;

        WeaponStr = 1f;
        Speed = 6f;
        Time = 5f;

        WeaponStrIncrease = 2f;
        TimeDecrease = 0.5f;
        scaleIncrease = new Vector3(0.05f, 0f, 0.05f);

        MinTime = 1.5f;

        Dmg = ps.Str * WeaponStr;

        delay = new WaitForSeconds(Time);
    }
    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.GameState == 1) {
            Vector2 curMoveVec = Vector2.left;
            if (ps.inputVec != Vector3.zero) curMoveVec = ps.inputVec;

            float angle = Mathf.Atan2(curMoveVec.y, curMoveVec.x) * Mathf.Rad2Deg;
            GameObject obj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle - 90f));
            obj.transform.localScale += (Lv - 1) * scaleIncrease;
            if (obj.transform.localScale.x > maxScale) obj.transform.localScale = Vector3.one * maxScale;
            obj.GetComponent<Wind>().inputVec = curMoveVec;

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
