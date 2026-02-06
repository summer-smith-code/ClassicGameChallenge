using UnityEngine;

public class Cove : MonoBehaviour
{
    // Check if cove is filled
    private bool isFilled = false;
    // Flag to prevent multiple triggers
    private bool hasTriggered = false;

    // Duck prefab for coves
    public GameObject duckPrefab;

    // Cove logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFilled && !hasTriggered)
        {
            // Log when the player reaches the cove
            Debug.Log("Player reached the cove and it is not filled.");

            // Set the cove to filled
            isFilled = true;
            // Prevent further triggers
            hasTriggered = true;  
            Debug.Log("Cove is now filled.");

            // Spawn the duck in the cove
            Quaternion duckRotation = Quaternion.Euler(0f, 180f, 0f);
            Vector3 spawnPosition = transform.position + new Vector3(0.5f, 0f, -1f);
            Instantiate(duckPrefab, spawnPosition, duckRotation);

            // Notify the player that the cove is filled
            other.GetComponent<PlayerController>().ReachedCove();
        }
        else if (other.CompareTag("Player") && isFilled)
        {
            // Log when the player reaches an already filled cove
            Debug.Log("Player reached the cove, but it is already filled.");
        }
    }

    //Keep cove from triggering again
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Log the player’s position
            Debug.Log("Player is staying in the cove. Position: " + other.transform.position);

            // Ensure the flag is set
            if (!hasTriggered && !isFilled)
            {
                Debug.Log("Setting cove to filled due to player stay.");
                isFilled = true;
                hasTriggered = true;

                // Spawn the duck
                Quaternion duckRotation = Quaternion.Euler(0f, 180f, 0f);
                Vector3 spawnPosition = transform.position + new Vector3(0.5f, 0f, -1f);
                Instantiate(duckPrefab, spawnPosition, duckRotation);

                other.GetComponent<PlayerController>().ReachedCove();
            }
        }
    }

    // Reset hasTriggered if the player exits trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasTriggered = false;
            Debug.Log("Player exited the cove, resetting hasTriggered.");
        }
    }

    // Other scripts see IsFilled
    public bool IsFilled => isFilled;
}
