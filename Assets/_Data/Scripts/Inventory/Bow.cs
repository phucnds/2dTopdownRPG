using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Animator anim;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        anim.SetTrigger(FIRE_HASH);
        Projectile arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        arrow.UpdateProjectileRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
