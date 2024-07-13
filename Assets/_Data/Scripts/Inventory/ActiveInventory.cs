using System;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    [SerializeField] private int activeSlotIndexNum = 0;

    private InputSystem inputActions;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputSystem();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        inputActions.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
        EquipStartingWeapon();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighLight(activeSlotIndexNum);
    }

    private void ToggleActiveSlot(int numValue) => ToggleActiveHighLight(numValue - 1);

    private void ToggleActiveHighLight(int indexNum)
    {
        Debug.Log(indexNum);

        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (PlayerHealth.Instance.IsDead) return;

        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        GameObject weapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        ActiveWeapon.Instance.NewWeapon(weapon.GetComponent<MonoBehaviour>());
    }
}
