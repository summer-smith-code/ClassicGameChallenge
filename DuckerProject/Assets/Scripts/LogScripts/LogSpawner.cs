using UnityEngine;

// Log spawner script
public class LogSpawner : MonoBehaviour
{
    //enter log prefabs and spawn interval in inspector
    public GameObject[] logPrefabs;
    public float spawnInterval = 2f;

    //set lane position and length
    public float spawnX = -25f;
    public float laneZ;

    //spawn logs at regular intervals
    void Start()
    {
        InvokeRepeating(nameof(SpawnLog), 0f, spawnInterval);
    }

    //spawn a random log prefab at the spawn position
    void SpawnLog()
    {
        //select random log prefab
        GameObject prefab =
            logPrefabs[Random.Range(0, logPrefabs.Length)];

        //set spawn position
        Vector3 spawnPos = new Vector3(
            spawnX,
            transform.position.y,
            laneZ
        );
        //create log instance
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
