using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    public bool GettingKnockBack { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource, float knockbackThrust)
    {
        GettingKnockBack = true;
        Vector2 dir = (transform.position - damageSource.position).normalized;
        Vector2 difference = dir * knockbackThrust * rb.mass;
        rb.velocity = Vector2.zero;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        GettingKnockBack = false;
    }
}
