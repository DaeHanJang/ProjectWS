using System.Collections;
using UnityEngine;

public class ClawState : WeaponState {
    private GameObject projectile = null;
    private PlayerMovement pm = null;
    private Vector3 increaseScale = new Vector3(0.05f, 0.05f, 0.05f);

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Claw");
    }

    protected override void Start() {
        pm = GameManager.Inst.player.GetComponent<PlayerMovement>();
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(7);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (cntProjectile < maxCntProjectile && level % 2 != 0) cntProjectile += increaseCntProjectile;
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 0f;
        minTime = 1f;
        maxCntProjectile = 5;
        maxCntHit = 0;
        weaponStrength = 2f;
        time = 3f;
        speed = 0f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 2f;
        increaseSpeed = 0f;
        decreaseTime = 0.2f;
        increaseCntProjectile = 1;
        increaseMaxCntHit = 0;
    }

    //¹«±â ÀåÂø
    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            Vector3 directionVec = Vector3.left;
            if (pm.moveVec.x > 0) directionVec = Vector3.right;
            for (int i = 0; i < cntProjectile; i++) {
                GameObject obj = Instantiate(projectile, transform.position + directionVec, Quaternion.identity);
                if (level <= 10) obj.transform.localScale += increaseScale * (level - 1);
                if (directionVec == Vector3.left) obj.GetComponent<SpriteRenderer>().flipX = true;
                directionVec *= -1;
                yield return new WaitUntil(() => obj == null);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
