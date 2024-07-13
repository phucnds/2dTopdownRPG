using System;
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private WeaponInfo weaponInfo;

    private Animator anim;
    private GameObject slashAnim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        slashAnim = Instantiate(slashAnimPrefab, PlayerController.Instance.SlashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.SetParent(transform.parent);
        PlayerController.Instance.WeaponCollider.gameObject.SetActive(true);
    }

    private void DoneAttackingAnimEvent()
    {
        PlayerController.Instance.WeaponCollider.gameObject.SetActive(false);
    }

    private void SwingUpFlipAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        slashAnim.GetComponent<SpriteRenderer>().flipX = PlayerController.Instance.FacingLeft;
    }

    private void SwingDownFlipAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        slashAnim.GetComponent<SpriteRenderer>().flipX = PlayerController.Instance.FacingLeft;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        ActiveWeapon.Instance.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
        PlayerController.Instance.WeaponCollider.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
