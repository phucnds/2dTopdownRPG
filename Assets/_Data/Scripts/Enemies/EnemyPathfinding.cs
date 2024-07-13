using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Knockback knockback;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<Knockback>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (knockback.GettingKnockBack) return;
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 pos)
    {
        movement = pos;
    }

    public void StopMoving()
    {
        movement = Vector2.zero;
    }

}
