using UnityEngine;

public class Destrucible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    private PickupSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<PickupSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<DamageSource>() || other.GetComponent<Projectile>())
        {
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            spawner.DropItems();
            Destroy(gameObject);
        }
    }
}
