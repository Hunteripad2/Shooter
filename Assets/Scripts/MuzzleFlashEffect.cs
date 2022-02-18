using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlashEffect : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 0.01f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
