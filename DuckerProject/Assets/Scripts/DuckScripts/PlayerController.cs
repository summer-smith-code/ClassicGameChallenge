using UnityEngine;

//Player controller (for cove/lives/death logic)
public class PlayerController : MonoBehaviour
{
    // Set start point
    public Transform startPoint;

    // Collision detection with obstacles (still adding)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit an obstacle!");
            GameManager.Instance.PlayerLostLife();
            TeleportToStart();
        }
    }

    // Teleport the player back to the start(still adding)
    private void TeleportToStart()
    {
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }

    // When player reaches a cove (win condition)
    public void ReachedCove()
    {
        Debug.Log("Player reached a cove.");
        GameManager.Instance.CoveFilled();
    }
}
