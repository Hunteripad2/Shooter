using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;

    [SerializeField] private GameObject doorToOpen;

    private void Start()
    {
        if (doorToOpen != null)
        {
            doorAnimator = doorToOpen.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (doorAnimator != null && collider.CompareTag("Player"))
        {
            doorAnimator.SetBool("character_nearby", true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (doorAnimator != null && collider.CompareTag("Player"))
        {
            doorAnimator.SetBool("character_nearby", false);
        }
    }
}
