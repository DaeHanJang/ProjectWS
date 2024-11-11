using System.Collections;
using UnityEngine;

public class ShieldState : WeaponState {
    private GameObject projectile = null;
    private GameObject shield = null;
    private Transform posPlayer = null;
    private Vector3 increaseScale = new Vector3(0.25f, 0.25f, 0.25f);
    private float activeTime = 20f;

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Shield");
    }

    protected override void Start() {
        posPlayer = GameManager.Inst.player.transform;
        shield = Instantiate(projectile, transform.position, Quaternion.identity);
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(3);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        if (cntProjectile < maxCntProjectile) cntProjectile += increaseCntProjectile;
        if (speed < maxSpeed) speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        if (level <= 20) shield.transform.localScale += increaseScale;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 0f;
        minTime = 3f;
        maxCntProjectile = 1;
        maxCntHit = 0;
        weaponStrength = 0.5f;
        time = 5f;
        speed = 0f;
        cntProjectile = 1;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 0.5f;
        increaseSpeed = 0f;
        decreaseTime = 0.5f;
        increaseCntProjectile = 0;
        increaseMaxCntHit = 0;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            shield.transform.position = posPlayer.transform.position;
            shield.SetActive(true);
            yield return new WaitForSeconds(activeTime / time);
            shield.SetActive(false);
            yield return new WaitForSeconds(time);
        }
    }
}
