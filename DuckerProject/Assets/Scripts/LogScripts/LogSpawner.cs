using UnityEngine;
using System.Collections;

// Log spawner script
public class LogSpawner : MonoBehaviour
{
    //log prefabs to spawn
    public GameObject[] logPrefabs;

    //spawn intervals
    private float[] spawnIntervals = { 1f, 2f, 3f };

    //spawn position
    public float spawnX = -25f;
    public float laneZ;

    //run spawner
    void Start()
    {
        StartCoroutine(SpawnLogs());
    }

    //spawn logs at random intervals
    IEnumerator SpawnLogs()
    {
        while (true)
        {
            SpawnLog();

            float waitTime = spawnIntervals[Random.Range(0, spawnIntervals.Length)];
            yield return new WaitForSeconds(waitTime);
        }
    }

    //spawn a single log
    void SpawnLog()
    {
        GameObject prefab =
            logPrefabs[Random.Range(0, logPrefabs.Length)];

        Vector3 spawnPos = new Vector3(
            spawnX,
            transform.position.y,
            laneZ
        );

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
