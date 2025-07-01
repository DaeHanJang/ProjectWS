using System;
using System.Collections.Generic;
using UnityEngine;

//Player waepon manager
public class PlayerWeapon : MonoBehaviour {
    //무기 종류
    public enum EWeapon {
        Bullet,
        Orb,
        Bomb,
        Shield,
        Wind,
        Thunder,
        Wall,
        Claw,
        Laser,
        Mine,
        Max,
        NULL
    };

    private Dictionary<int, WeaponFactory> wf = new Dictionary<int, WeaponFactory>();
    public HashSet<int> weaponIdx = new HashSet<int>();
    public const int maxWeaponCnt = 5;
    public int weaponCnt = 0;

    private void Start() {
        SetWeapon(UnityEngine.Random.Range(0, (int)EWeapon.Max));
    }

    public void SetWeapon(int idx) {
        if (!wf.ContainsKey(idx)) {
            switch (GetWeaponType(idx)) {
                case EWeapon.Bullet: wf.Add(idx, gameObject.AddComponent<BulletFactory>()); break;
                case EWeapon.Orb: wf.Add(idx, gameObject.AddComponent<OrbFactory>()); break;
                case EWeapon.Bomb: wf.Add(idx, gameObject.AddComponent<BombFactory>()); break;
                case EWeapon.Shield: wf.Add(idx, gameObject.AddComponent<ShieldFactory>()); break;
                case EWeapon.Wind: wf.Add(idx, gameObject.AddComponent<WindFactory>()); break;
                case EWeapon.Thunder: wf.Add(idx, gameObject.AddComponent<ThunderFactory>()); break;
                case EWeapon.Wall: wf.Add(idx, gameObject.AddComponent<WallFactory>()); break;
                case EWeapon.Claw: wf.Add(idx, gameObject.AddComponent<ClawFactory>()); break;
                case EWeapon.Laser: wf.Add(idx, gameObject.AddComponent<LaserFactory>()); break;
                case EWeapon.Mine: wf.Add(idx, gameObject.AddComponent<MineFactory>()); break;
            }
            weaponIdx.Add(idx);
            ++weaponCnt;
        }
        else wf[idx].LvUp();
    }

    public EWeapon GetWeaponType(int idx) {
        switch (idx) {
            case 0: return EWeapon.Bullet;
            case 1: return EWeapon.Orb;
            case 2: return EWeapon.Bomb;
            case 3: return EWeapon.Shield;
            case 4: return EWeapon.Wind;
            case 5: return EWeapon.Thunder;
            case 6: return EWeapon.Wall;
            case 7: return EWeapon.Claw;
            case 8: return EWeapon.Laser;
            case 9: return EWeapon.Mine;
            default: return EWeapon.NULL;
        }
    }

    public void DestoryWeapon() {
        foreach (var comp in wf) comp.Value.enabled = false;
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject obj in projectiles) Destroy(obj);
    }

    public int GetEWeaponMax() {
        return (int)EWeapon.Max;
    }

    public string GetWeaponName(int idx) {
        return Enum.GetName(typeof(EWeapon), idx);
    }
}
