using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public Transform startPoint;
    private bool isOnLog = false;

    //check tags on obstacles
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("Player hit a Car!");
            GameManager.Instance.PlayerLostLife();
            TeleportToStart();
        }
        else if (other.CompareTag("Water") && !isOnLog)
        {
            Debug.Log("Player fell in Water!");
            GameManager.Instance.PlayerLostLife();
            TeleportToStart();
        }
        else if (other.CompareTag("Cove"))
        {
            ReachedCove();
        }
    }

    //override water death when on log
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            isOnLog = true;
        }
    }

    //player dies in water(not on log)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            isOnLog = false;
        }
    }

    //reset player position and stop velocity
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

    public void ReachedCove()
    {
        Debug.Log("Player reached a cove.");
        GameManager.Instance.CoveFilled();
        TeleportToStart();
    }
}
