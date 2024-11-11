using System.Collections;
using UnityEngine;

public class WallState : WeaponState {
    private GameObject projectile = null;
    private GameObject wall = null;
    private Vector3 increaseScale = new Vector3(0.05f, 0.05f, 0.05f);
    private float activeTime = 20f;

    private void Awake() {
        projectile = Resources.Load<GameObject>("Weapons/Wall");
    }

    protected override void Start() {
        wall = Instantiate(projectile, transform.position, Quaternion.identity);
        base.Start();
    }

    [ContextMenu("LevelUp")]
    public override void LevelUp() {
        pw.AddWeaponLevelList(6);
        ++level;
        weaponStrength += increaseWeaponStrength;
        maxCntHit += increaseMaxCntHit;
        cntProjectile += increaseCntProjectile;
        speed += increaseSpeed;
        if (time > minTime) time -= decreaseTime;
        if (wall.transform.localScale.x < 0.8f) wall.transform.localScale += increaseScale;
        strength = ps.str * weaponStrength;
    }

    protected override void SetState() {
        maxSpeed = 0f;
        minTime = 3f;
        maxCntProjectile = 0;
        maxCntHit = 0;
        weaponStrength = 5f;
        time = 5f;
        speed = 0f;
        cntProjectile = 0;
        strength = ps.str * weaponStrength;
    }

    protected override void SetIncreaseState() {
        increaseWeaponStrength = 5f;
        increaseSpeed = 0f;
        decreaseTime = 0.5f;
        increaseCntProjectile = 0;
        increaseMaxCntHit = 0;
    }

    protected override IEnumerator SetWeapon() {
        while (GameManager.Inst.gameState == 1) {
            wall.SetActive(true);
            yield return new WaitForSeconds(activeTime / time);
            wall.SetActive(false);
            yield return new WaitForSeconds(time);
        }
    }
}
