using UnityEngine;

//ÆøÅº
public class Bomb : MonoBehaviour {
    private GameObject particle = null;
    private BombState bs = null;

    public Vector3 posDestination;

    private void Awake() {
        particle = Resources.Load<GameObject>("Weapons/BombExplosion");
    }

    private void Start() {
        bs = GameManager.Inst.player.GetComponent<BombState>();
    }

    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, posDestination, bs.speed * Time.deltaTime);

        if (transform.position == posDestination) {
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
