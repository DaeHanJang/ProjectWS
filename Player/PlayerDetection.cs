using UnityEngine;

//�÷��̾� �ֺ� ����
public class PlayerDetection : MonoBehaviour {
    private CircleCollider2D cc = null;
    private LayerMask layer; //������ �� ���̾�

    public Collider2D[] enemy; //������ �� �ݶ��̴� �迭
    public GameObject closestEnemy = null; //���� ����� �� ������Ʈ

    private void Awake() {
        cc = GetComponent<CircleCollider2D>();
        layer = LayerMask.GetMask("Enemy");
    }

    private void Update() {
        enemy = Physics2D.OverlapCircleAll(transform.position, cc.radius, layer); //�������� �� �ݶ��̴� ����

        if (enemy.Length > 0) { //������ ���� ���� ��� ���� ����� �� ����
            closestEnemy = enemy[0].gameObject;
            float closestDistance = Vector2.Distance(transform.position, enemy[0].transform.position);
            foreach (Collider2D c in enemy) {
                float closestDistance2 = Vector2.Distance(transform.position, c.transform.position);
                if (closestDistance > closestDistance2) {
                    closestDistance = closestDistance2;
                    closestEnemy = c.gameObject;
                }
            }
        }
    }
}
