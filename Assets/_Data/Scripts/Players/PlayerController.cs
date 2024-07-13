using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCDTime = .5f;
    [SerializeField] private TrailRenderer trailRenderer;
    [field: SerializeField] public Transform SlashAnimSpawnPoint { get; private set; }
    [field: SerializeField] public Transform WeaponCollider { get; private set; }

    private InputSystem inputActions;
    private Vector2 movement;
    private float startingMoveSpeed;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer mySprite;
    private Knockback knockback;

    private bool facingLeft;
    private bool isDashing;

    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    protected override void Awake()
    {
        base.Awake();

        inputActions = new InputSystem();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        inputActions.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = inputActions.Movement.Move.ReadValue<Vector2>();

        anim.SetFloat("moveX", movement.x);
        anim.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockBack || PlayerHealth.Instance.IsDead) return;

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        mySprite.flipX = mousePos.x < playerScreenPoint.x;
        FacingLeft = mousePos.x < playerScreenPoint.x;
    }

    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            trailRenderer.emitting = true;
            Stamina.Instance.UseStamina();
            StartCoroutine(EndDashRoutine());
        }

    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCDTime);
        isDashing = false;
    }

}
