
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float roamRange = 1f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCD = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private enum State
    {
        Roaming,
        Attacking
    }

    private bool canAttack = true;

    private Vector2 roamPos;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;


    private void Awake()
    {
        state = State.Roaming;
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    private void Start()
    {
        roamPos = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:

            case State.Roaming:
                Roam();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roam()
    {
        timeRoaming += Time.deltaTime;
        enemyPathfinding.MoveTo(roamPos);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) <= attackRange)
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPos = GetRoamingPosition();
            timeRoaming = 0f;
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            Vector2 pos = stopMovingWhileAttacking ? Vector2.zero : GetRoamingPosition();
            enemyPathfinding.MoveTo(pos);

            StartCoroutine(AttackCoolDownRoutine());
        }
    }

    private IEnumerator AttackCoolDownRoutine()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-roamRange, roamRange), Random.Range(-roamRange, roamRange)).normalized;
    }
}

