using UnityEngine;

//플레이어 주변 감지
public class PlayerDetection : MonoBehaviour {
    private CircleCollider2D cc = null;
    private LayerMask layer; //감지할 적 레이어

    public Collider2D[] enemy; //감지된 적 콜라이더 배열
    public GameObject closestEnemy = null; //가장 가까운 적 오브젝트

    private void Awake() {
        cc = GetComponent<CircleCollider2D>();
        layer = LayerMask.GetMask("Enemy");
    }

    private void Update() {
        enemy = Physics2D.OverlapCircleAll(transform.position, cc.radius, layer); //원형으로 적 콜라이더 감지

        if (enemy.Length > 0) { //감지된 적이 있을 경우 가장 가까운 적 갱신
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
