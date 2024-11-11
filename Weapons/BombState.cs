using System.Collections;
using UnityEngine;

public class BombState : WeaponState {
    private GameObject projectile = null;
    private float radius = 3f;

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Bomb");
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(2);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (level % 2 != 0 && cntProjectile < maxCntProjectile) cntProjectile += increaseCntProjectile;
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 10f;
        minTime = 1f;
        maxCntProjectile = 5;
        maxCntHit = 30;
        weaponStrength = 10f;
        time = 5f;
        speed = 6f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 5f;
        increaseSpeed = 0.5f;
        decreaseTime = 0.5f;
        increaseCntProjectile = 1;
        increaseMaxCntHit = 5;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            for (int i = 0; i < cntProjectile; i++) {
                float angle = Random.Range(0, 360);
                var radian = angle * Mathf.Deg2Rad;
                float x = Mathf.Cos(radian) * radius;
                float y = Mathf.Sin(radian) * radius;
                Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Bomb>().posDestination = transform.position + new Vector3(x, y);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
