using UnityEngine;

public class Cove : MonoBehaviour
{
    // Check if cove is filled
    private bool isFilled = false;
    // Flag to prevent multiple triggers
    private bool hasTriggered = false;

    // Duck that sits in filled coves
    public GameObject duck;

    // Cove logic
    private void Awake()
    {
        // Ensure the duck is invisible when the game starts
        if (duck != null)
        {
            duck.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFilled && !hasTriggered)
        {
            Debug.Log("Player reached the cove and it is not filled.");

            // Set the cove to filled
            isFilled = true;
            // Prevent further triggers
            hasTriggered = true;
            Debug.Log("Cove is now filled.");

            // Make the duck visible
            if (duck != null)
            {
                duck.SetActive(true);
            }

            // Notify the player that the cove is filled
            other.GetComponent<PlayerController>().ReachedCove();
        }
        else if (other.CompareTag("Player") && isFilled)
        {
            Debug.Log("Player reached the cove, but it is already filled.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is staying in the cove. Position: " + other.transform.position);

            if (!hasTriggered && !isFilled)
            {
                Debug.Log("Setting cove to filled due to player stay.");
                isFilled = true;
                hasTriggered = true;

                // Make the duck visible
                if (duck != null)
                {
                    duck.SetActive(true);
                }

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
