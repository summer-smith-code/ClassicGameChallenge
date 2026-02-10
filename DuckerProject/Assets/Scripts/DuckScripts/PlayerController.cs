using UnityEngine;
using System.Collections.Generic;

// PlayerCollision Controller
public class PlayerController : MonoBehaviour
{
    public Transform startPoint;
    public AudioClip[] example;

    // Tracks log collisions
    private bool isOnLog = false;
    private bool touchingLogThisFrame = false;

    // Tracks which rows the player has already scored for this life
    private HashSet<int> visitedRows = new HashSet<int>();

    private void Start()
    {
        visitedRows.Clear();

        // Add starting row so player doesn't get points immediately
        int startingRow = Mathf.RoundToInt(transform.position.z);
        visitedRows.Add(startingRow);
    }

    //tracks collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GameManager.Instance.PlayerLostLife();
            TeleportToStart();
            visitedRows.Clear();

            // Mark starting row to prevent immediate points
            int startingRow = Mathf.RoundToInt(transform.position.z);
            visitedRows.Add(startingRow);
        }
        else if (other.CompareTag("Cove"))
        {
            ReachedCove();
            visitedRows.Clear();

            // Mark starting row for next run
            int startingRow = Mathf.RoundToInt(transform.position.z);
            visitedRows.Add(startingRow);
        }
    }
    //dont die in water if on log
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Log"))
            touchingLogThisFrame = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log"))
            isOnLog = false;
    }

    private void FixedUpdate()
    {
        isOnLog = touchingLogThisFrame;
        touchingLogThisFrame = false;

        // Check for water death
        if (!isOnLog)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].CompareTag("Water"))
                {
                    GameManager.Instance.PlayerLostLife();
                    TeleportToStart();
                    visitedRows.Clear();

                    // Add starting row again to avoid immediate points
                    int startingRow = Mathf.RoundToInt(transform.position.z);
                    visitedRows.Add(startingRow);
                    break;
                }
            }
        }

        // Handle scoring for new rows
        int currentRow = Mathf.RoundToInt(transform.position.z);
        if (!visitedRows.Contains(currentRow))
        {
 
            visitedRows.Add(currentRow);
            GameManager.Instance.PlayerHopped();
        }
    }

    //teleport player to start
    private void TeleportToStart()
    {
        SoundFXManager.Instance.PlaySound(example, transform, 0.5f, 0);
        DuckerMovement duckerMovement = GetComponent<DuckerMovement>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;

        if (duckerMovement != null)
        {
            duckerMovement.targetPosition = new Vector3(
                Mathf.Round(transform.position.x),
                duckerMovement.yOffset,
                Mathf.Round(transform.position.z)
            );
            transform.position = duckerMovement.targetPosition;
        }

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    //called when player reaches cove
    public void ReachedCove()
    {
        SoundFXManager.Instance.PlaySound(example, transform, 0.5f, 0);
        GameManager.Instance.CoveFilled();
        TeleportToStart();


        visitedRows.Clear();
        int startingRow = Mathf.RoundToInt(transform.position.z);
        visitedRows.Add(startingRow);
    }
}
