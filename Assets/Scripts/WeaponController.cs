using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [HideInInspector] private Camera mainCamera;
    [HideInInspector] private PlayerMovement player;
    [HideInInspector] private Vector3 updatePositionVelocity;
    [HideInInspector] public enum ActionState { idle, aiming, reloading, meeleAttacking }
    [HideInInspector] public int currentActionState = (int)ActionState.idle;
    [HideInInspector] private float reloadTime;
    [HideInInspector] private float angleX;
    [HideInInspector] private float angleY;
    [HideInInspector] private Transform effectsFolder;
    [HideInInspector] public float recoilVelocity;
    [HideInInspector] private int currentWeaponId;
    [HideInInspector] public WeaponComponent currentWeapon;

    [Header("General")]
    [SerializeField] private WeaponComponent[] weapons;

    [Header("Movement")]
    [SerializeField] private float positionLag = 0.005f;

    [Header("Shooting")]
    [SerializeField] private GameObject wallHitDecal;
    [SerializeField] private float wallHitOffset = 0.01f;
    [SerializeField] private float recoilLossSpeed = 10f;
    [SerializeField] private GameObject[] muzzleFlashes;
    [SerializeField] public float recoilZMult = -0.25f;
    [SerializeField] public LayerMask ignoreLayer;
    [SerializeField] private GameObject hitWallSoundObject;
    [SerializeField] private GameObject hitEnemySoundObject;

    [Header("Position and Rotation")]
    [SerializeField] public Vector3 defaultPosition = new Vector3(0.2f, -0.2f, -0.1f);
    [SerializeField] public Vector3 aimPosition = new Vector3(0f, -0.18f, -0.2f);
    [SerializeField] public Vector3 hidePosition = new Vector3(0.2f, -0.5f, -0.5f);
    [SerializeField] public Vector3 defaultRotation = new Vector3(0f, 0f, 0f);
    [SerializeField] public Vector3 reloadRotation = new Vector3(10f, 10f, 0f);

    private void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        effectsFolder = GameObject.FindGameObjectWithTag("Effects Folder").transform;

        currentWeapon = weapons[0];
    }

    private void Update()
    {
        HandleRecoil();

        HandleSwitching();

        if (currentActionState == (int)ActionState.reloading)
        {
            ContinueReloading();
        }
        else if (currentActionState == (int)ActionState.meeleAttacking)
        {
            //TODO
        }
        else
        {
            HandleShooting();
            HandleAiming();
            HandleReloading();
        }

        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.SmoothDamp(transform.position, mainCamera.transform.position, ref updatePositionVelocity, 0f);
        if (currentActionState != (int)ActionState.aiming)
        {
            currentWeapon.transform.localPosition += -updatePositionVelocity * positionLag;
        }
    }

    private void UpdateRotation()
    {
        transform.rotation = mainCamera.transform.rotation;
    }

    private void HandleRecoil()
    {
        if (recoilVelocity > 0.1f)
        {
            recoilVelocity = Mathf.Lerp(recoilVelocity, 0f, Time.deltaTime * recoilLossSpeed);
        }
        else
        {
            recoilVelocity = 0f;
        }
    }

    private void HandleSwitching()
    {
        if (Input.GetButtonDown("Switch Weapon"))
        {
            if (currentWeaponId == 0)
            {
                currentWeaponId = 1;
            }
            else
            {
                currentWeaponId = 0;
            }

            currentWeapon = weapons[currentWeaponId];

            currentActionState = (int)ActionState.idle;
        }
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && currentWeapon.fireCooldown <= 0f)
        {
            if (currentWeapon.magazineAmmo != 0)
            {
                Shoot();
            }
            else if (currentWeapon.pocketAmmo != 0 || currentWeapon.pocketSize == 0)
            {
                StartReloading();
            }
            else
            {
                //TODO
            }
        }
    }

    private void Shoot()
    {
        int randomSoundId = Random.Range(0, currentWeapon.shootSoundEffects.Length);
        currentWeapon.shootSoundEffects[randomSoundId].Play();

        currentWeapon.fireCooldown = currentWeapon.fireCooldownTime;
        currentWeapon.magazineAmmo -= 1;

        int randomFlashId = Random.Range(0, muzzleFlashes.Length);
        Instantiate(muzzleFlashes[randomFlashId], currentWeapon.muzzle.position, currentWeapon.muzzle.rotation * Quaternion.Euler(0, 0, 90), effectsFolder);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, currentWeapon.shootingRange, ~ignoreLayer))
        {
            if (rayHit.transform.CompareTag("Wall"))
            {
                GameObject hitSound = Instantiate(hitWallSoundObject, rayHit.point, Quaternion.identity, rayHit.transform);
                Destroy(hitSound, 1f);
                Instantiate(wallHitDecal, rayHit.point + rayHit.normal * wallHitOffset, Quaternion.LookRotation(rayHit.normal), rayHit.transform);
            }
            else if (rayHit.transform.CompareTag("Enemy"))
            {
                GameObject hitSound = Instantiate(hitEnemySoundObject, rayHit.point, Quaternion.identity, rayHit.transform);
                Destroy(hitSound, 1f);
                rayHit.collider.GetComponent<HealthManagement>().TakeDamage(currentWeapon.damage);
                Instantiate(wallHitDecal, rayHit.point + rayHit.normal * wallHitOffset, Quaternion.LookRotation(rayHit.normal), rayHit.transform);
            }
        }

        recoilVelocity += currentWeapon.recoilForce;
    }

    private void HandleReloading()
    {
        if (Input.GetButtonDown("Reload") && currentWeapon.magazineAmmo != currentWeapon.magazineSize && (currentWeapon.pocketAmmo != 0 || currentWeapon.pocketSize == 0))
        {
            StartReloading();
        }
    }

    private void StartReloading()
    {
        currentActionState = (int)ActionState.reloading;
        reloadTime = currentWeapon.reloadTime;
        currentWeapon.reloadSoundEffect.Play();
    }

    private void ContinueReloading()
    {
        reloadTime -= Time.deltaTime;
        if (reloadTime <= 0)
        {
            FinishReloading();
        }
    }

    private void FinishReloading()
    {
        currentActionState = (int)ActionState.idle;

        int ammoToReload = currentWeapon.magazineSize - currentWeapon.magazineAmmo;
        if (currentWeapon.pocketAmmo < ammoToReload && currentWeapon.pocketSize != 0)
        {
            ammoToReload = currentWeapon.pocketAmmo;
        }

        currentWeapon.magazineAmmo += ammoToReload;
        if (currentWeapon.pocketSize != 0)
        {
            currentWeapon.pocketAmmo -= ammoToReload;
        }
    }

    private void HandleAiming()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (currentActionState == (int)ActionState.idle)
            {
                currentActionState = (int)ActionState.aiming;
            }
            else if (currentActionState == (int)ActionState.aiming)
            {
                currentActionState = (int)ActionState.idle;
            }
        }

        if (currentActionState == (int)ActionState.aiming && player.velocity.y != player.defaultVelocityY)
        {
            currentActionState = (int)ActionState.idle;
        }
    }
}