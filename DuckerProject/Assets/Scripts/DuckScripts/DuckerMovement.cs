using UnityEngine;

public class DuckerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;       // Tile movement speed
    public float yOffset = 0.34f;      // Frog height above floor/log
    public float rotationSpeed = 720f; // Degrees/sec

    private Vector3 targetPosition;
    private Vector3 moveDirection = Vector3.forward;

    private Transform currentLog;
    private Vector3 previousLogPosition;

    // Threshold to detect log wrapping (larger than largest log length)
    private float wrapThreshold = 5f;

    void Start()
    {
        // Snap frog to nearest grid at start
        targetPosition = new Vector3(Mathf.Round(transform.position.x),
                                     yOffset,
                                     Mathf.Round(transform.position.z));
        transform.position = targetPosition;
    }

    void Update()
    {
        HandleInput();
        MoveTowardsTarget();
        RideLog();
        RotateTowardsMovement();
    }

    void HandleInput()
    {
        // Only allow movement if frog reached target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Vector3 input = Vector3.zero;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                input = Vector3.forward;
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                input = Vector3.back;
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                input = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                input = Vector3.right;

            if (input != Vector3.zero)
            {
                moveDirection = input.normalized;
                targetPosition += input;
                targetPosition.y = yOffset;
            }
        }
    }

    void MoveTowardsTarget()
    {
        // Smoothly slide to the target tile
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsMovement()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    void RideLog()
    {
        if (currentLog != null)
        {
            Vector3 logDelta = currentLog.position - previousLogPosition;

            // Ignore huge jumps caused by log wrapping
            if (Mathf.Abs(logDelta.x) > wrapThreshold)
                logDelta.x = 0;
            if (Mathf.Abs(logDelta.z) > wrapThreshold)
                logDelta.z = 0;

            // Add log movement to frog (both position and targetPosition)
            transform.position += new Vector3(logDelta.x, 0, logDelta.z);
            targetPosition += new Vector3(logDelta.x, 0, logDelta.z);

            previousLogPosition = currentLog.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            currentLog = other.transform;
            previousLogPosition = currentLog.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log") && other.transform == currentLog)
        {
            currentLog = null;
        }
    }
}
