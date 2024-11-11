using UnityEngine;

//Scene������ �����Ǵ� Singleton Component
public class SceneSingletonComponent<T> : MonoBehaviour where T : MonoBehaviour {
    private static T inst = null;

    public static T Inst {
        get { return inst; }
    }

    protected virtual void Awake() {
        if (inst == null) inst = gameObject.GetComponent<T>();
        else Destroy(gameObject);
    }
}
