//Character status
public interface IStatus {
    int Lv { get; set; } //레벨
    float Exp { get; set; } //현재 경험치
    float MaxExp { get; set; } //최대 경험치
    float Hp { get; set; } //현재 체력
    float MaxHp { get; set; } //최대 체력
    float Str { get; set; } //공격력
    float Def { get; set; } //방어력
    float DefCoe { get; set; } //방어 계수
    float Speed { get; set; } //이동 속도
    float ExpIncrease { get; set; } //경험치 증가량
    float HpIncrease { get; set; } //체력 증가량
    float StrIncrease { get; set; } //공격력 증가량
    float DefIncrease { get; set; } //방어력 증가량
    float SpeedIncrease { get; set; } //이동 속도 증가량
}
