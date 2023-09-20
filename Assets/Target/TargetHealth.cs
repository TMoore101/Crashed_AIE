using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TargetHealth : MonoBehaviour
{
    public float health = 25;
    public float maxHealth = 25;

    public GameObject deathParticles;

    [SerializeField]
    private AudioClip[] DeathNoises;

    private void Update()
    {
        if (health <= 0)
        {
            GameObject deathParticle = Instantiate(deathParticles);
            deathParticle.transform.position = transform.position;

            GameObject audio = new GameObject("SFX", typeof(AudioSource));
            audio.GetComponent<AudioSource>().outputAudioMixerGroup = GetComponent<AudioSource>().outputAudioMixerGroup;
            audio.GetComponent<AudioSource>().clip = DeathNoises[Random.Range(0, DeathNoises.Length)];
            audio.GetComponent<AudioSource>().Play();
            Destroy(audio, audio.GetComponent<AudioSource>().clip.length);

            Object.Destroy(gameObject);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        /*if (health < maxHealth)
        {
            if (GetComponent<EnemyMovement>() != null)
            {
                GameManager.Instance.groupDetection[GetComponent<EnemyMovement>().enemyGroup] = true;
            }
            else if (GetComponent<EnemyMovementShip>() != null)
            {
                GetComponent<EnemyMovementShip>().player = GetComponent<EnemyMovementShip>().realPlayer;
            }
        }*/
    }
}
