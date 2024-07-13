using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }


    private void Update()
    {
        if (particle && !particle.IsAlive())
        {
            DestroySelf();
        }
    }
    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
