using System;
using System.Collections;
using UnityEngine;

//번개 스테이터스
public class ThunderState : WeaponState {
    private GameObject projectile = null;
    private PlayerDetection pd = null;

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Thunder");
    }

    protected override void Start() {
        pd = GameManager.Inst.pd;
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(5);
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
        minTime = 0.8f;
        maxCntProjectile = 25;
        maxCntHit = 1;
        weaponStrength = 1f;
        time = 3f;
        speed = 0f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 2f;
        increaseSpeed = 0f;
        decreaseTime = 0.2f;
        increaseCntProjectile = 5;
        increaseMaxCntHit = 0;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            if (pd.closestEnemy) {
                for (int i = 0; i < cntProjectile; i++) {
                    try {
                        Instantiate(projectile, pd.enemy[i].transform.position, Quaternion.identity);
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
