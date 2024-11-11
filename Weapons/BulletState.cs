using System;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

//총알 스테이터스
public class BulletState : WeaponState {
    private GameObject projectile = null;
    private PlayerDetection pd = null; //플레이어 주변 감지 컴포넌트

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Bullet");
    }

    protected override void Start() {
        pd = GameManager.Inst.pd;
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(0);
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
        minTime = 0.3f;
        maxCntProjectile = 1;
        maxCntHit = 1;
        weaponStrength = 2f;
        time = 1.5f;
        speed = 6f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 2f;
        increaseSpeed = 0.5f;
        decreaseTime = 0.1f;
        increaseCntProjectile = 0;
        increaseMaxCntHit = 0;
    }

    //무기 장착
    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            if (pd.closestEnemy) {
                for (int i = 0; i < cntProjectile; i++) {
                    try {
                        float angle = Mathf.Atan2(pd.enemy[i].transform.position.y - transform.position.y,
                        pd.enemy[i].transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                        Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle + 90f)).GetComponent<Bullet>().posEnemy = pd.enemy[i].transform;
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
