using UnityEngine;

//CoveScript
public class Cove : MonoBehaviour
{
    //Check if cove is filled
    private bool isFilled = false;
    //duck prefab for coves
    public GameObject duckPrefab;

    //Cove logic
    private void OnTriggerEnter(Collider other)
    {
        //Check if cove is reached
        if (other.CompareTag("Player") && !isFilled)
        {
            Debug.Log("Player reached the cove!");
            isFilled = true;

            //Spawn duck in cove facing player
            Quaternion duckRotation = Quaternion.Euler(0f, 180f, 0f);

            //puts duck in center of cove
            Vector3 spawnPosition = transform.position + new Vector3(0.5f, 0f, -1f);

            Instantiate(duckPrefab, spawnPosition, duckRotation);

            //tells player if cove is full

            other.GetComponent<PlayerController>().ReachedCove();
        }
    }

    public bool IsFilled => isFilled;
}
