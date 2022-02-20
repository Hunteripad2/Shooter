using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private GameObject soundObject;
    [SerializeField] private float destroyTime = 2f;

    private void Start()
    {
        soundObject = Instantiate(soundObject, transform.position, Quaternion.identity, transform);

        AudioSource[] soundEffects = soundObject.GetComponents<AudioSource>();
        int randomEffectId = Random.Range(0, soundEffects.Length);

        soundEffects[randomEffectId].Play();
        Destroy(gameObject, destroyTime);
    }
}
