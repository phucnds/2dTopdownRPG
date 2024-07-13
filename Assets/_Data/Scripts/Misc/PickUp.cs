using System.Collections;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin = 0,
        HealthGlobe = 1,
        StaminaGlobe = 2
    }

    [SerializeField] private PickUpType type;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelartionRate = .2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    [SerializeField] private float range = 1.5f;

    private Vector3 moveDir;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if (Vector3.Distance(playerPos, transform.position) <= pickUpDistance)
        {
            moveDir = playerPos - transform.position;
            moveSpeed += accelartionRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            DetectPickType();
            Destroy(gameObject);
        }
    }

    private void DetectPickType()
    {
        switch (type)
        {

            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentCoin();
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealthPlayer();
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                break;
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPos = transform.position;

        float randomX = transform.position.x + Random.Range(-range, range);
        float randomY = transform.position.y + Random.Range(-range, range);

        Vector2 endPos = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);

            transform.position = Vector2.Lerp(startPos, endPos, linearT) + new Vector2(0, height);
            yield return null;
        }
    }

}
