using System.Collections;
using UnityEngine;

//���� �������ͽ�
public abstract class WeaponState : MonoBehaviour {
    protected PlayerState ps = null;
    protected PlayerWeapon pw = null;
    protected float increaseWeaponStrength = 0f; //���� ���� ���ݷ� ������
    protected float increaseSpeed = 0f; //�ӵ� ������
    protected float decreaseTime = 0f; //��� �ð� ���Ұ�
    protected int increaseCntProjectile = 0; //����ü �� ������
    protected int increaseMaxCntHit = 0; //�ִ� ���� ���� �� ������

    protected float maxSpeed = 0f; //�ִ� �ӵ�
    protected float minTime = 0f; //�ּ� ��� �ð�
    protected int maxCntProjectile = 1; //�ִ� ����ü ��
    public int maxCntHit = 1; //�ִ� ���� ���� ��

    protected float weaponStrength = 0f; //���� ���� ���ݷ�
    protected float time = 0f; //��� �ð�

    public int level = 1; //����
    public float strength = 0f; //���� ���ݷ�
    public float speed = 0f; //�ӵ�
    public int cntProjectile = 1; //����ü ��

    protected virtual void Start() {
        ps = GameManager.Inst.ps;
        pw = GameManager.Inst.pw;
        SetState();
        SetIncreaseState();
        StartCoroutine(SetWeapon());
    }

    public abstract void LevelUp(); //���� ��
    protected abstract void SetState(); //���� �������ͽ� ����
    protected abstract void SetIncreaseState(); //���� ������ ����
    protected abstract IEnumerator SetWeapon(); //���� ����
}
