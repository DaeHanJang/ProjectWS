using UnityEngine;

//Bomb
public class Bomb : Weapon {
    private GameObject particle = null;

    public Vector3 destinationPos;

    private void Awake() {
        wf = GameObject.Find("Player").GetComponent<BombFactory>();
        particle = Resources.Load<GameObject>("Weapons/BombExplosion");
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, destinationPos, wf.Speed * Time.deltaTime);

        if (transform.position == destinationPos) {
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
