using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "WeaponInfo", order = 0)]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float attackCD;
    public int weaponDamage;
    public int weaponRange;
}