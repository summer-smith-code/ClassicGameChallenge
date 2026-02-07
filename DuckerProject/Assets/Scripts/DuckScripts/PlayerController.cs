using UnityEngine;
using System.Collections;  // Add this line to fix the IEnumerator error

public class PlayerController : MonoBehaviour
{
    public Transform startPoint;
    private bool isOnLog = false;
    private bool isLeavingLog = false;  // Flag to prevent immediate exit detection

    // Check for tags on obstacles
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name);

        // Check if the player hits a car
        if (other.CompareTag("Car"))
        {
            Debug.Log("Player hit a Car!");
            GameManager.Instance.PlayerLostLife();
            TeleportToStart();
        }
        // Check if the player falls in water (only trigger death if not on a log)
        else if (other.CompareTag("Water"))
        {
            if (!isOnLog)  // Only die if we're not on a log
            {
                Debug.Log("Player fell in Water!");
                GameManager.Instance.PlayerLostLife();
                TeleportToStart();
            }
            else
            {
                Debug.Log("Player is on a Log, skipping water death.");
            }
        }
        // Check if the player reaches a cove
        else if (other.CompareTag("Cove"))
        {
            ReachedCove();
        }
    }

    // Continuously check if the player is on a log
    private void OnTriggerStay(Collider other)
    {
        // If the player is on the log, and we haven't already set the flag
        if (other.CompareTag("Log"))
        {
            if (!isOnLog)  // Only set it to true if it's not already set
            {
                isOnLog = true;  // Player is on the log, safe from water death
                Debug.Log("Player is on a Log");
            }
            isLeavingLog = false;  // Reset leaving log flag when staying on it
        }
    }

    // Player exits the log (set isOnLog to false)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            // Add a margin of safety before the player is considered to have left the log
            if (!isLeavingLog)
            {
                StartCoroutine(WaitAndCheckExit(other));  // Use a delay to check exit more reliably
            }
        }
    }

    private IEnumerator WaitAndCheckExit(Collider other)
    {
        yield return new WaitForSeconds(0.1f);  // Small delay before setting exit state

        if (!isLeavingLog)  // Make sure the player is still outside the log after the delay
        {
            isOnLog = false;  // Player has completely left the log
            Debug.Log("Player left the Log");
            isLeavingLog = true;
        }
    }

    // Reset player position and stop velocity
    private void TeleportToStart()
    {
        DuckerMovement duckerMovement = GetComponent<DuckerMovement>();
        if (duckerMovement != null)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;

            duckerMovement.targetPosition = new Vector3(Mathf.Round(transform.position.x), duckerMovement.yOffset, Mathf.Round(transform.position.z));
            transform.position = duckerMovement.targetPosition;

            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    // Player reached a cove (complete level or goal)
    public void ReachedCove()
    {
        Debug.Log("Player reached a cove.");
        GameManager.Instance.CoveFilled();
        TeleportToStart();
    }
}
