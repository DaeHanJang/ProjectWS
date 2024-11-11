using UnityEngine;

//Game전체에서 유지되는 Singleton Component
public class GameSingletonComponent<T> : MonoBehaviour where T : MonoBehaviour {
    private static T inst = null;

    public static T Inst {
        get {
            if (!inst) {
                inst = FindObjectOfType<T>();

                if (!inst) {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    inst = obj.GetComponent<T>();
                }
            }
            return inst;
        }
    }

    protected virtual void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
