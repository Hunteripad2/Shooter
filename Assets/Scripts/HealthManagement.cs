using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    [HideInInspector] private int healthAmount;
    [HideInInspector] private Transform effectsFolder;

    [Header("Characteristics")]
    [SerializeField] protected int maxHealth = 1;

    [Header("Effects")]
    [SerializeField] private GameObject[] deathEffects;
    [SerializeField] private AudioSource damageSoundEffect;

    private void Start()
    {
        healthAmount = maxHealth;
        effectsFolder = GameObject.FindGameObjectWithTag("Effects Folder").transform;
    }

    public void TakeDamage(int damage)
    {
        damageSoundEffect.Play();
        healthAmount -= damage;

        if (healthAmount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int randomEffectId = Random.Range(0, deathEffects.Length);
        Instantiate(deathEffects[randomEffectId], transform.position, Quaternion.identity, effectsFolder);
        Destroy(gameObject);
    }
}
