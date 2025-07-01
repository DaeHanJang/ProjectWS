using System.Collections;
using UnityEngine;

//Weapon state
public abstract class WeaponFactory : MonoBehaviour, IWeaponStatus {
    protected GameObject projectile = null; //Projectile obj.
    protected PlayerState ps = null; //Player state
    protected PlayerWeapon pw = null; //Player weapon manager
    protected Coroutine co = null; //Set weapon coroutine
    protected WaitForSeconds delay = null; //Weapon delay time

    public int Lv { get; set; }
    public float Dmg { get; set; }
    public float WeaponStr { get; set; }
    public int HitCnt { get; set; }
    public float Speed { get; set; }
    public int ProjectileCnt { get; set; }
    public float Time { get; set; }
    public float WeaponStrIncrease { get; set; }
    public int HitCntIncrease { get; set; }
    public float SpeedIncrease { get; set; }
    public int ProjectileCntIncrease { get; set; }
    public float TimeDecrease { get; set; }
    public int MaxHitCnt { get; set; }
    public float MaxSpeed { get; set; }
    public int MaxProjectileCnt { get; set; }
    public float MinTime { get; set; }

    protected virtual void Awake() {
        ps = GameObject.Find("Player").GetComponent<PlayerState>();
        pw = GameObject.Find("Player").GetComponent<PlayerWeapon>();
    }

    protected virtual void Start() {
        InitState();
        co = StartCoroutine(SetWeapon());
    }
    protected abstract void InitState();
    protected abstract IEnumerator SetWeapon();
    public abstract void LvUp();
}
