using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GrapeProjectile grapeProjectilePrefab;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        anim.SetTrigger(ATTACK_HASH);
        spriteRenderer.flipX = PlayerController.Instance.transform.position.x - transform.position.x < 0;
    }

    private void SpawnProjectileAnimEvent()
    {
        GrapeProjectile grapeProjectile = Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
    }
}
