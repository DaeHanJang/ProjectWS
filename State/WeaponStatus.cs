//Weapon status
public interface IWeaponStatus {
    int Lv { get; set; } //����
    float Dmg { get; set; } //���� ������

    float WeaponStr { get; set; } //���� ���ݷ�
    int HitCnt { get; set; } //Ÿ�� Ƚ��
    float Speed { get; set; } //�ӵ�
    int ProjectileCnt { get; set; } //�߻�ü ����
    float Time { get; set; } //�ð�

    float WeaponStrIncrease { get; set; } //���� ���ݷ� ������
    int HitCntIncrease { get; set; } //Ÿ�� Ƚ�� ������
    float SpeedIncrease { get; set; } //�ӵ� ������
    int ProjectileCntIncrease { get; set; } //�߻�ü ���� ������
    float TimeDecrease { get; set; } //�ð� ���ҷ�

    int MaxHitCnt { get; set; } //�ִ� Ÿ�� Ƚ��
    float MaxSpeed { get; set; } //�ִ� �ӵ�
    int MaxProjectileCnt {  get; set; } //�ִ� �߻�ü ����
    float MinTime { get; set; } //�ּ� �ð�
}
