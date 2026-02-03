using System.Collections;
using UnityEngine;

// Ducker movement script
public class DuckerMovement : MonoBehaviour
{
    // Movement settings
    public float moveDistance = 1f;
    public float moveCooldown = 0.2f;
    public float leapDuration = 0.125f;

    // Grid boundaries
    public int minX = -7;
    public int maxX = 7;
    public int minZ = -7;
    public int maxZ = 7;

    // Movement tracking
    private float lastMoveTime;
    private Vector2Int gridPosition;

    // Log tracking
    private Transform currentLog = null;
    private Vector3 lastLogPosition;

    //movement logic
    void Start()
    {
        gridPosition = new Vector2Int(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z)
        );
    }


    void Update()
    {
        // Move duck with log if standing on one
        if (currentLog != null)
        {
            Vector3 logDelta = currentLog.position - lastLogPosition;
            transform.position += logDelta;

            // Keep duck on top of the log
            transform.position = new Vector3(
                transform.position.x,
                currentLog.position.y + 0.5f,
                transform.position.z
            );

            lastLogPosition = currentLog.position;
        }

        // Check move cooldown
        if (Time.time - lastMoveTime < moveCooldown)
            return;

        // WASD or Arrow Key Input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            TryMove(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            TryMove(Vector3.back);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(Vector3.right);
    }


    //move attempt logic
    void TryMove(Vector3 direction)
    {
        Vector2Int moveDir = new Vector2Int(
            Mathf.RoundToInt(direction.x),
            Mathf.RoundToInt(direction.z)
        );
        // Target grid position
        Vector2Int targetGridPos = gridPosition + moveDir;

        // Block out-of-bounds moves
        if (targetGridPos.x < minX || targetGridPos.x > maxX ||
            targetGridPos.y < minZ || targetGridPos.y > maxZ)
        {
            return;
        }

        // Rotate duck
        transform.forward = direction;

        // Commit grid move
        gridPosition = targetGridPos;

        // World position to leap to
        Vector3 targetWorldPos = new Vector3(
            gridPosition.x,
            transform.position.y,
            gridPosition.y
        );

        // Start leap coroutine
        StartCoroutine(Leap(targetWorldPos));
        lastMoveTime = Time.time;
    }

    //leap movement logic
    private IEnumerator Leap(Vector3 destination)
    {
        //move animation
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        //leap over time
        while (elapsedTime < leapDuration)
        {
            transform.position = Vector3.Lerp(startPosition, destination, elapsedTime / leapDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //check final position
        transform.position = destination;
    }

    //log collision logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            currentLog = other.transform;
            lastLogPosition = currentLog.position;

            // Snap duck onto top of the log
            Vector3 snapPos = new Vector3(
                transform.position.x,
                currentLog.position.y + 0.5f,
                transform.position.z
            );
            transform.position = snapPos;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log") && currentLog == other.transform)
        {
            currentLog = null;
        }
    }
}
