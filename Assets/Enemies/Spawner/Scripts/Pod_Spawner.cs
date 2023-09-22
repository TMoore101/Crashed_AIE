using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pod_Spawner : MonoBehaviour
{
    public GameObject podPrefab;
    public Transform spawnLocation;
    private Animator podAnim;
    private bool podSpawned;

    public GameObject enemyPrefab;
    public float enemyCount;
    public float spawnRange;
    private Vector3 spawnPoint;
    private bool enemySpawned;

    public LayerMask groundLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (podPrefab != null && !podSpawned)
        {
            GameObject pod = Instantiate(podPrefab, spawnLocation.position, spawnLocation.rotation);
            pod.transform.parent = spawnLocation;
            podSpawned = true;

            if (!enemySpawned)
            {
                StartCoroutine(SpawnEnemy(pod));
            }
        }
    }

    IEnumerator SpawnEnemy(GameObject pod)
    {
        enemySpawned = true;
        yield return new WaitForSeconds(2);
        for (int i = 0; i < enemyCount; i++)
        {
            //Calculate random point in range
            float randomZ = Random.Range(-spawnRange, spawnRange);
            float randomX = Random.Range(-spawnRange, spawnRange);

            spawnPoint = new Vector3(pod.transform.position.x + randomX, pod.transform.position.y + 1, pod.transform.position.z + randomZ);
            if (Physics.Raycast(spawnPoint, -transform.up, 4f, groundLayer))
            {
                Instantiate(enemyPrefab, spawnPoint, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
    }
}
