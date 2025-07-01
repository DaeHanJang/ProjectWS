//Weapon status
public interface IWeaponStatus {
    int Lv { get; set; } //레벨
    float Dmg { get; set; } //최종 데미지

    float WeaponStr { get; set; } //무기 공격력
    int HitCnt { get; set; } //타격 횟수
    float Speed { get; set; } //속도
    int ProjectileCnt { get; set; } //발사체 개수
    float Time { get; set; } //시간

    float WeaponStrIncrease { get; set; } //무기 공격력 증가량
    int HitCntIncrease { get; set; } //타격 횟수 증가량
    float SpeedIncrease { get; set; } //속도 증가량
    int ProjectileCntIncrease { get; set; } //발사체 개수 증가량
    float TimeDecrease { get; set; } //시간 감소량

    int MaxHitCnt { get; set; } //최대 타격 횟수
    float MaxSpeed { get; set; } //최대 속도
    int MaxProjectileCnt {  get; set; } //최대 발사체 개수
    float MinTime { get; set; } //최소 시간
}
