using System.Collections;
using UnityEngine;

public class MineState : WeaponState {
    private GameObject projectile = null;
    private Vector3 increaseScale = new Vector3(0.5f, 0.5f, 0.5f);
    private float decreaseTimer = 0.25f;
    private float minTimer = 0.5f;

    public Vector3 BoomScale = new Vector3(1f, 1f, 1f);
    public float timer = 0f;

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Mine");
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(9);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (cntProjectile < maxCntProjectile) cntProjectile += increaseCntProjectile;
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        if (timer > minTimer) timer -= decreaseTimer;
        if (BoomScale.x < 4f && level % 2 != 0) BoomScale += increaseScale;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 0f;
        minTime = 1f;
        maxCntProjectile = 1;
        maxCntHit = 0;
        weaponStrength = 20f;
        time = 1f;
        speed = 0f;
        cntProjectile = 1;
        timer = 1.5f;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 5f;
        increaseSpeed = 0f;
        decreaseTime = 0.5f;
        increaseCntProjectile = 0;
        increaseMaxCntHit = 0;
    }

    //¹«±â ÀåÂø
    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            Instantiate(projectile, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(time);
        }
    }
}
