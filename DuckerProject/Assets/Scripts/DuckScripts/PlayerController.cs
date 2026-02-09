using UnityEngine;
using System.Collections;

//PlayerCollision Controller
public class PlayerController : MonoBehaviour
{
    public Transform startPoint;
    public AudioClip[] example;

    //Tracks log collisions
    private bool isOnLog = false;
    private bool touchingLogThisFrame = false;

    // Detects collisions with cars, coves, logs, and water
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GameManager.Instance.PlayerLostLife();
            TeleportToStart();
        }
        else if (other.CompareTag("Cove"))
        {
            ReachedCove();
        }
    }
    // Detects if player is on a log
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            touchingLogThisFrame = true;
        }
    }

    // Detects when player leaves a log
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            isOnLog = false;
        }
    }

    private void Update()
    {
        // If player was on a log last frame but isn't this frame, check for water collision
        isOnLog = touchingLogThisFrame;
        touchingLogThisFrame = false;

       
        if (!isOnLog)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].CompareTag("Water"))
                {
                    GameManager.Instance.PlayerLostLife();
                    TeleportToStart();
                    break;
                }
            }
        }
    }

    // Teleports player to starting position and resets velocity
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

    // Handles player reaching a cove, notifying GameManager and teleporting to start
    public void ReachedCove()
    {
        SoundFXManager.Instance.PlaySound(example, transform, 0.5f, 0);
        GameManager.Instance.CoveFilled();
        TeleportToStart();
    }
}
