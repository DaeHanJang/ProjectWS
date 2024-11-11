using System;
using System.Collections.Generic;
using UnityEngine;

//플레이어 무기 관리
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
        Max
    };

    private List<MonoBehaviour> weapons = new List<MonoBehaviour>(); //소유 무기 컴포넌트
    private Dictionary<int, int> weaponOrder = new Dictionary<int, int>(); //소유 무기 순서
    private Dictionary<int, int> weaponLevel = new Dictionary<int, int>(); //소유 무기 레벨

    public HashSet<int> weaponsIdx = new HashSet<int>(); //소유 무기 고유 번호
    public int maxWeaponCnt = 5; //최대 소유 무기 갯수
    public int weaponCnt = 0; //소유 무기 갯수

    private void Start() {
        SetWeapon(UnityEngine.Random.Range(0, (int)EWeapon.Max));
    }

    //무기 장착
    public void SetWeapon(int idx) {
        if (weaponsIdx.Contains(idx)) {
            switch (idx) { 
                case 0: GetComponent<BulletState>().LevelUp(); break;
                case 1: GetComponent<OrbState>().LevelUp(); break;
                case 2: GetComponent<BombState>().LevelUp(); break;
                case 3: GetComponent<ShieldState>().LevelUp(); break;
                case 4: GetComponent<WindState>().LevelUp(); break;
                case 5: GetComponent<ThunderState>().LevelUp(); break;
                case 6: GetComponent<WallState>().LevelUp(); break;
                case 7: GetComponent<ClawState>().LevelUp(); break;
                case 8: GetComponent<LaserState>().LevelUp(); break;
                case 9: GetComponent<MineState>().LevelUp(); break;
                default: break;
            }
        }
        else { //소유하지 않은 무기일 경우
            weaponsIdx.Add(idx); //무기 고유 번호 추기
            weaponOrder.Add(idx, weaponCnt++); //고유 번호를 키값으로 무기 순서 설정
            weaponLevel.Add(idx, 1); //무기 레벨 1 저장
            switch (idx) { //무기 컴포넌트 적재
                case 0: weapons.Add(gameObject.AddComponent<BulletState>()); break;
                case 1: weapons.Add(gameObject.AddComponent<OrbState>()); break;
                case 2: weapons.Add(gameObject.AddComponent<BombState>()); break;
                case 3: weapons.Add(gameObject.AddComponent<ShieldState>()); break;
                case 4: weapons.Add(gameObject.AddComponent<WindState>()); break;
                case 5: weapons.Add(gameObject.AddComponent<ThunderState>()); break;
                case 6: weapons.Add(gameObject.AddComponent<WallState>()); break;
                case 7: weapons.Add(gameObject.AddComponent<ClawState>()); break;
                case 8: weapons.Add(gameObject.AddComponent<LaserState>()); break;
                case 9: weapons.Add(gameObject.AddComponent<MineState>()); break;
                default: break;
            }
        }
    }

    public void AddWeaponLevelList(int idx) {
        weaponLevel[idx]++;
    }

    public void DestoryWeapon() {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Weapon");
        foreach (GameObject obj in projectiles) Destroy(obj);
    }

    public int GetWeaponeTypeCnt() {
        return (int)EWeapon.Max;
    }

    public int GetWeaponIdx(string name) {
        return (int)Enum.Parse(typeof(EWeapon), name);
    }

    public string GetWeaponName(int idx) {
        return Enum.GetName(typeof(EWeapon), idx);
    }
}
