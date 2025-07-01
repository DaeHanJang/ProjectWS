using UnityEngine;

//Player radar
public class PlayerDetection : MonoBehaviour {
    private CircleCollider2D cc = null; //Collier comp.
    private LayerMask layer; //Layer to detect

    public Collider2D[] enemy = null;
    public GameObject nearestEnemy = null;

    private void Awake() {
        cc = GetComponent<CircleCollider2D>();
        layer = LayerMask.GetMask("Enemy");
    }

    public GameObject Detect() {
        enemy = Physics2D.OverlapCircleAll(transform.position, cc.radius, layer);

        if (enemy.Length > 0) {
            nearestEnemy = enemy[0].gameObject;
            float nearestEnemyDistance = Vector2.Distance(transform.position, nearestEnemy.transform.position);

            foreach (Collider2D c in enemy) {
                float tmp = Vector2.Distance(transform.position, c.transform.position);

                if (nearestEnemyDistance > tmp) {
                    nearestEnemy = c.gameObject;
                    nearestEnemyDistance = tmp;
                }
            }
        }

        return nearestEnemy;
    }
}
