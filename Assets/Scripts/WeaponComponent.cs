using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponComponent : MonoBehaviour
{
    [HideInInspector] private WeaponController weaponController;
    [HideInInspector] private TextMesh pocketAmmoCountText;
    [HideInInspector] private TextMesh magazineAmmoCountText;
    [HideInInspector] private Vector3 positionVelocity;
    [HideInInspector] private Vector3 rotationVelocity;
    [HideInInspector] private Vector3 rotationChange;
    [HideInInspector] private Vector3 reloadPositionVelocity;
    [HideInInspector] public float fireCooldown;
    [HideInInspector] public int pocketAmmo;
    [HideInInspector] public int magazineAmmo;

    [Header("Characteristics")]
    [SerializeField] public float fireCooldownTime = 0.1f;
    [SerializeField] public float reloadTime = 2f;
    [SerializeField] public int pocketSize = 0;
    [SerializeField] public int magazineSize = 0;
    [SerializeField] public float recoilForce = 40f;
    [SerializeField] public float shootingRange = 100f;

    [Header("Ammo UI")]
    [SerializeField] private GameObject pocketAmmoCount;
    [SerializeField] private GameObject magazineAmmoCount;

    [Header("Shooting")]
    [SerializeField] public Transform muzzle;

    [Header("Position and Rotation")]
    [SerializeField] public float repositionTime = 0.1f;

    private void Start()
    {
        weaponController = GameObject.FindGameObjectWithTag("Weapon Controller").GetComponent<WeaponController>();

        if (pocketAmmoCount != null)
        {
            pocketAmmoCountText = pocketAmmoCount.GetComponent<TextMesh>();
        }
        if (magazineAmmoCount != null)
        {
            magazineAmmoCountText = magazineAmmoCount.GetComponent<TextMesh>();
        }

        transform.localPosition = weaponController.hidePosition;

        pocketAmmo = pocketSize;
        magazineAmmo = magazineSize;
    }

    private void Update()
    {
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }

        UpdatePosition();

        UpdateUI();
    }

    private void UpdatePosition()
    {
        Vector3 targetPosition = weaponController.hidePosition;
        Vector3 targetRotation = weaponController.defaultRotation;

        if (weaponController.currentWeapon == this)
        {
            targetPosition = weaponController.defaultPosition;

            if (weaponController.currentActionState == (int)WeaponController.ActionState.aiming)
            {
                targetPosition = weaponController.aimPosition;
            }
            else if (weaponController.currentActionState == (int)WeaponController.ActionState.reloading)
            {
                targetRotation = weaponController.reloadRotation;
            }

            targetPosition += new Vector3(0, 0, weaponController.recoilVelocity * weaponController.recoilZMult * Time.deltaTime);
        }

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref positionVelocity, repositionTime);
        transform.localRotation = Quaternion.Euler(Vector3.SmoothDamp(transform.localRotation.eulerAngles, targetRotation, ref rotationVelocity, repositionTime));
    }

    private void UpdateUI()
    {
        //if (magazineAmmoCountText != null)
        //{
        magazineAmmoCountText.text = magazineAmmo.ToString();
        //}

        if (pocketAmmoCountText != null)
        {
            pocketAmmoCountText.text = pocketAmmo.ToString();
        }
    }
}