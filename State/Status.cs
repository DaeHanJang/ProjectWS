//Character status
public interface IStatus {
    int Lv { get; set; } //����
    float Exp { get; set; } //���� ����ġ
    float MaxExp { get; set; } //�ִ� ����ġ
    float Hp { get; set; } //���� ü��
    float MaxHp { get; set; } //�ִ� ü��
    float Str { get; set; } //���ݷ�
    float Def { get; set; } //����
    float DefCoe { get; set; } //��� ���
    float Speed { get; set; } //�̵� �ӵ�
    float ExpIncrease { get; set; } //����ġ ������
    float HpIncrease { get; set; } //ü�� ������
    float StrIncrease { get; set; } //���ݷ� ������
    float DefIncrease { get; set; } //���� ������
    float SpeedIncrease { get; set; } //�̵� �ӵ� ������
}
