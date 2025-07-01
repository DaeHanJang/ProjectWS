using UnityEngine;

//Player die state
public class PlayerDie : State {
    private PlayerState ps = null;
    private AudioSource ads = null;

    private void Awake() {
        owner = GameObject.Find("Player");
        at = owner.GetComponent<Animator>();
        ps = owner.GetComponent<PlayerState>();
        ads = owner.GetComponent<AudioSource>();
    }

    public override void OnStateEnter() {
        ps.IsDie = true;
        owner.GetComponent<PlayerWeapon>().DestoryWeapon();
        GameManager.Inst.GameOver();
        at.SetTrigger("Die");
        ads.Play();
        Invoke("DestroyOwner", ads.clip.length);
    }

    public override void OnStateUpdate() { }

    public override void OnStateExit() { }

    private void DestroyOwner() {
        Destroy(owner);
    }
}
