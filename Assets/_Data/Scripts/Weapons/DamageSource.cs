using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount;

    private void Start()
    {
        MonoBehaviour curWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (curWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            enemy.TakeDamage(damageAmount);
        }
    }
}
