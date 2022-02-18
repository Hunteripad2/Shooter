using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private WeaponController weaponController;
    private float rotationX = 0f;
    private float zoomVelocity;

    [Header("General")]
    [SerializeField] private Camera cam;
    [SerializeField] private float mouseSensitivity = 200f;

    [Header("FOW")]
    [SerializeField] private float defaultFow = 60f;
    [SerializeField] private float zoomFow = 45f;
    [SerializeField] private float zoomTime = 0.1f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weaponController = GameObject.FindGameObjectWithTag("Weapon Controller").GetComponent<WeaponController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //if (!inventoryObject.isShown && !pauseMenu.isShown)
        //{
        HandleRotation();

        HandleZoom();
        //}
        //else if (Cursor.lockState == CursorLockMode.Locked)
        //{
        //    Cursor.lockState = CursorLockMode.Confined;
        //}
    }

    private void HandleRotation()
    {
        if (Cursor.lockState == CursorLockMode.Confined)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime + weaponController.recoilVelocity * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX - mouseY, -70f, 70f);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        player.Rotate(Vector3.up, mouseX);
    }

    private void HandleZoom()
    {
        if (weaponController.currentActionState == (int)WeaponController.ActionState.aiming)
        {
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, zoomFow, ref zoomVelocity, zoomTime);
        }
        else
        {
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, defaultFow, ref zoomVelocity, zoomTime);
        }
    }
}
