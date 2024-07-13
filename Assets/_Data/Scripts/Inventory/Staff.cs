using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private MagicLaser magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator anim;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        anim.SetTrigger(ATTACK_HASH);
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        MagicLaser newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.UpdateLaserRange(weaponInfo.weaponRange);
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        ActiveWeapon.Instance.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
