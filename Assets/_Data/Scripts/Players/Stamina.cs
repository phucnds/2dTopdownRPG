using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    [SerializeField] private Sprite fullStaImg, emptyStaImg;
    [SerializeField] private float timeBetweenStaminaRefresh = 3f;

    public int CurrentStamina { get; private set; }

    private int startingStamina = 3;
    private int maxStamina;
    private Transform staminaContainer;

    const string STAMINA_CONTAINER = "StaminaContainer";

    protected override void Awake()
    {
        base.Awake();
        maxStamina = startingStamina;
        CurrentStamina = maxStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();

        StopAllCoroutines();
        StartCoroutine(StaminaRefreshRoutine());
    }

    public void RefreshStamina()
    {
        if (PlayerHealth.Instance.IsDead) return;
        CurrentStamina++;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
        UpdateStaminaImages();
    }

    public void ReplenishStaminaOnDeath()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

    private IEnumerator StaminaRefreshRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    private void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            staminaContainer.GetChild(i).GetComponent<Image>().sprite = i <= CurrentStamina - 1 ? fullStaImg : emptyStaImg;
        }
    }
}
