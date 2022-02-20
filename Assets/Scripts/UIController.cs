using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [HideInInspector] private WeaponController weaponController;
    [HideInInspector] private Image[] crosshairArrows;

    [Header("Elements")]
    [SerializeField] private GameObject reloadSpinner;
    [SerializeField] private GameObject crosshair;

    [Header("Colors")]
    [SerializeField] private Color red = new Color(255f, 0f, 0f, 1f);
    [SerializeField] private Color blue = new Color(0f, 233f, 255f, 1f);

    private void Start()
    {
        weaponController = GameObject.FindGameObjectWithTag("Weapon Controller").GetComponent<WeaponController>();
        crosshairArrows = crosshair.GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }
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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;


            if (Physics.Raycast(ray, out rayHit, weaponController.currentWeapon.shootingRange, ~weaponController.ignoreLayer) && rayHit.transform.CompareTag("Enemy"))
            {
                foreach (Image crosshairArrow in crosshairArrows)
                {
                    crosshairArrow.color = red;
                }
            }
            else
            {
                foreach (Image crosshairArrow in crosshairArrows)
                {
                    crosshairArrow.color = blue;
                }
            }
        }
        else
        {
            crosshair.SetActive(false);
        }
    }
}
