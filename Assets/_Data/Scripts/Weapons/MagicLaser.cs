using System.Collections;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsule;
    private float laserRange;
    private bool isGrowing = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Indestrucible>() && !other.isTrigger)
        {
            isGrowing = false;
        }
    }

    public void UpdateLaserRange(float range)
    {
        laserRange = range;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linerT = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linerT), 1f);

            capsule.size = new Vector2(Mathf.Lerp(1f, laserRange, linerT), capsule.size.y);
            capsule.offset = new Vector2(Mathf.Lerp(1f, laserRange, linerT) / 2, capsule.offset.y);
            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = transform.position - mousePosition;
        transform.right = -direction;
    }
}
