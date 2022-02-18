using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [HideInInspector] private WeaponController weaponController;

    [Header("Elements")]
    [SerializeField] private GameObject reloadSpinner;
    [SerializeField] private GameObject crosshair;

    private void Start()
    {
        weaponController = GameObject.FindGameObjectWithTag("Weapon Controller").GetComponent<WeaponController>();
    }

    private void FixedUpdate()
    {
        if (weaponController.currentActionState == (int)WeaponController.ActionState.reloading)
        {
            reloadSpinner.SetActive(true);
        }
        else
        {
            reloadSpinner.SetActive(false);
        }

        if (weaponController.currentActionState == (int)WeaponController.ActionState.idle)
        {
            crosshair.SetActive(true);
        }
        else
        {
            crosshair.SetActive(false);
        }
    }
}
