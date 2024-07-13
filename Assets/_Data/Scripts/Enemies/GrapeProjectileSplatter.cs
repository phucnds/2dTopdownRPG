using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectileSplatter : MonoBehaviour
{
    private SpriteFade spriteFade;

    private void Awake()
    {
        spriteFade = GetComponent<SpriteFade>();
    }

    private void Start()
    {
        StartCoroutine(spriteFade.SlowFadeRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.TakeDamage(1, transform);
        }
    }
}
