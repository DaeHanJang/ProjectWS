using System.Collections;
using UnityEngine;

public class WindState : WeaponState {
    private GameObject projectile = null;
    private PlayerMovement pm = null;
    private Vector3 increaseScale = new Vector3(0.05f, 0f, 0.05f);

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Wind");
    }

    protected override void Start() {
        pm = GameManager.Inst.player.GetComponent<PlayerMovement>();
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(4);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (cntProjectile < maxCntProjectile) cntProjectile += increaseCntProjectile;
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 10f;
        minTime = 1f;
        maxCntProjectile = 1;
        maxCntHit = 0;
        weaponStrength = 5f;
        time = 3f;
        speed = 6f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 3f;
        increaseSpeed = 0.5f;
        decreaseTime = 0.5f;
        increaseCntProjectile = 0;
        increaseMaxCntHit = 0;
    }


    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            Vector2 curMoveVec = pm.moveVec;
            if (pm.moveVec == Vector2.zero) curMoveVec = Vector2.left;
            float angle = Mathf.Atan2(curMoveVec.y, curMoveVec.x) * Mathf.Rad2Deg;
            GameObject obj = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle - 90f));
            if (level <= 10) obj.transform.localScale += increaseScale * (level - 1);
            obj.GetComponent<Wind>().inputVec = curMoveVec;
            yield return new WaitForSeconds(time);
        }
    }
}
