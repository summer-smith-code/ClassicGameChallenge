using UnityEngine;
using System.Collections;

// car spawner script
public class CarSpawner : MonoBehaviour
{
    //car prefabs to spawn
    public GameObject[] carPrefabs;

    //spawn intervals
    private float[] spawnIntervals = { 1f, 2f, 3f };

    //spawn position
    public float spawnX = -16f;
    public float laneZ;

    //run spawner
    void Start()
    {
        StartCoroutine(SpawnCars());
    }

    //spawn cars at random intervals
    IEnumerator SpawnCars()
    {
        while (true)
        {
            SpawnCar();

            float waitTime = spawnIntervals[Random.Range(0, spawnIntervals.Length)];
            yield return new WaitForSeconds(waitTime);
        }
    }

    //spawn a single car
    void SpawnCar()
    {
        GameObject prefab =
            carPrefabs[Random.Range(0, carPrefabs.Length)];

        Vector3 spawnPos = new Vector3(
            spawnX,
            transform.position.y,
            laneZ
        );

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
