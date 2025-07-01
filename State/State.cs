using UnityEngine;

//State Pattern
public abstract class State : MonoBehaviour {
    protected GameObject owner = null; //Owner obj.
    protected Animator at = null; //Owner's animator comp.

    public GameObject Owner { get; set; }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}
