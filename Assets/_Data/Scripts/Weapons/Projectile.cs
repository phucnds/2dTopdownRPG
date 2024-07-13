using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private GameObject particleOnHit;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float range)
    {
        projectileRange = range;
    }

    public void UpdateMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(startPos, transform.position) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        Indestrucible indestruct = other.GetComponent<Indestrucible>();
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (!other.isTrigger && (enemy || indestruct || player))
        {
            if (player && isEnemyProjectile || (enemy && !isEnemyProjectile))
            {
                player?.TakeDamage(1, transform);
                Instantiate(particleOnHit, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if(!other.isTrigger && indestruct)
            {
                Instantiate(particleOnHit, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }


    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }
}
