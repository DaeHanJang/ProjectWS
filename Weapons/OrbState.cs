using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbState : WeaponState {
    private List<GameObject> projectile = new List<GameObject>();
    private GameObject preProjectile = null;
    private float radius = 1.5f;

    private void Awake() {
        preProjectile = Resources.Load<GameObject>("Weapons/Orb");
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(1);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (level % 2 != 0 && cntProjectile < maxCntProjectile) {
            cntProjectile += increaseCntProjectile;
            projectile.Add(Instantiate(preProjectile, transform.position, Quaternion.identity));
        }
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 5f;
        minTime = 1f;
        maxCntProjectile = 6;
        maxCntHit = 0;
        weaponStrength = 1f;
        time = 0f;
        speed = 2f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
        projectile.Add(Instantiate(preProjectile, transform.position, Quaternion.identity));
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 1f;
        increaseSpeed = 0.5f;
        decreaseTime = 0f;
        increaseCntProjectile = 1;
        increaseMaxCntHit = 0;
    }

    protected override IEnumerator SetWeapon() {
        float angle = 0f;
        while (GameManager.Inst.gameState == 1) {
            if (Time.timeScale != 0f) {
                angle += speed;
                if (angle < 360f) {
                    for (int i = 0; i < cntProjectile; i++) {
                        var radian = (angle + (i * (360 / cntProjectile))) * Mathf.Deg2Rad;
                        float x = Mathf.Cos(radian) * radius;
                        float y = Mathf.Sin(radian) * radius;
                        projectile[i].transform.position = transform.position + new Vector3(x, y);
                    }
                }
                else angle = 0f;
            }
            yield return null;
        }
    }
}
