using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleEffect : MonoBehaviour
{
    [HideInInspector] private Material bulletHole;

    [Header("Bullet Hole")]
    [SerializeField] private float fadingSpeed = 1f;
    [SerializeField] private float minOpacity = 0.01f;

    private void Start()
    {
        bulletHole = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (bulletHole.color.a <= minOpacity)
        {
            Destroy(gameObject);
        }

        Color bulletHoleColor = bulletHole.color;

        bulletHoleColor.a = Mathf.Lerp(bulletHoleColor.a, 0f, Time.deltaTime * fadingSpeed);

        bulletHole.color = bulletHoleColor;
    }
}
