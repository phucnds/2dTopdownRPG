using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float knockBackThrust = 5f;
    [SerializeField] private float damageRecoveryTime = 5f;
    [SerializeField] private Slider slider;

    private Knockback knockback;
    private Animator anim;
    private Flash flash;
    private int currentHealth;
    private bool canTakeDamage = true;

    const string SLIDER_HEALTH = "Slider Health";
    const string TOWN = "Scene_3";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    public bool IsDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;
        UpdateSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);

        }
    }

    public void HealthPlayer()
    {
        currentHealth++;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateSlider();
    }

    public void TakeDamage(int damage, Transform hitTransform)
    {
        if (!canTakeDamage) return;
        ScreenShakeManager.Instance.ShakenScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PassAway();
        UpdateSlider();
        StartCoroutine(DamageRecoveryRoutine());
    }

    private void PassAway()
    {
        if (currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            anim.SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        Stamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(TOWN);
    }


    private void UpdateSlider()
    {
        if (slider == null)
        {
            slider = GameObject.Find(SLIDER_HEALTH).GetComponent<Slider>();
        }
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

}
