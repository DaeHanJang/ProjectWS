using System.Collections;
using UnityEngine;

//무기 스테이터스
public abstract class WeaponState : MonoBehaviour {
    protected PlayerState ps = null;
    protected PlayerWeapon pw = null;
    protected float increaseWeaponStrength = 0f; //무기 고유 공격력 증가값
    protected float increaseSpeed = 0f; //속도 증가값
    protected float decreaseTime = 0f; //대기 시간 값소값
    protected int increaseCntProjectile = 0; //투사체 수 증가량
    protected int increaseMaxCntHit = 0; //최대 공격 가능 수 증가량

    protected float maxSpeed = 0f; //최대 속도
    protected float minTime = 0f; //최소 대기 시간
    protected int maxCntProjectile = 1; //최대 투사체 수
    public int maxCntHit = 1; //최대 공격 가능 수

    protected float weaponStrength = 0f; //무기 고유 공격력
    protected float time = 0f; //대기 시간

    public int level = 1; //레벨
    public float strength = 0f; //최종 공격력
    public float speed = 0f; //속도
    public int cntProjectile = 1; //투사체 수

    protected virtual void Start() {
        ps = GameManager.Inst.ps;
        pw = GameManager.Inst.pw;
        SetState();
        SetIncreaseState();
        StartCoroutine(SetWeapon());
    }

    public abstract void LevelUp(); //레벨 업
    protected abstract void SetState(); //무기 스테이터스 설정
    protected abstract void SetIncreaseState(); //무기 증가량 설정
    protected abstract IEnumerator SetWeapon(); //무기 장착
}
