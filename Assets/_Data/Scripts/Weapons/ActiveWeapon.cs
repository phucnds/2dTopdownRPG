using System;
using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private InputSystem inputActions;
    private bool attackButtonDown, isAttacking = false;
    private float timeBetweenAttack;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputSystem();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        inputActions.Combat.Attack.started += _ => StartAttacking();
        inputActions.Combat.Attack.canceled += _ => StopAttacking();
        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttack = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().attackCD;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttack);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }
}
