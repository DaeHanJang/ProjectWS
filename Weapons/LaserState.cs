using System;
using System.Collections;
using UnityEngine;

public class LaserState : WeaponState {
    private GameObject projectile = null;
    private PlayerDetection pd = null; //플레이어 주변 감지 컴포넌트

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Laser");
    }

    protected override void Start() {
        pd = GameManager.Inst.pd;
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(8);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (cntProjectile < maxCntProjectile) cntProjectile += increaseCntProjectile;
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 0f;
        minTime = 1f;
        maxCntProjectile = 1;
        maxCntHit = 0;
        weaponStrength = 3f;
        time = 5f;
        speed = 0f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 3f;
        increaseSpeed = 0f;
        decreaseTime = 0.5f;
        increaseCntProjectile = 0;
        increaseMaxCntHit = 0;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            if (pd.closestEnemy) {
                for (int i = 0; i < cntProjectile; i++) {
                    try {
                        float angle = Mathf.Atan2(pd.enemy[i].transform.position.y - transform.position.y,
                        pd.enemy[i].transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle - 90f));
                    }
                    catch (IndexOutOfRangeException e) {
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(time);
        }
    }
}
