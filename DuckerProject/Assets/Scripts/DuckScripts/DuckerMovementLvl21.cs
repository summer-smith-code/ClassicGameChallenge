using UnityEngine;
using DG.Tweening;
//Ducker Movement

public class DuckerMovementLvl2 : MonoBehaviour
{
    //Ducker movespeed/startinglocation
    public float moveSpeed = 8f;
    public float yOffset = 0.34f;
    public float rotationSpeed = 720f;

    public Vector3 targetPosition;
    public Vector3 moveDirection = Vector3.forward;

    public Transform currentLog;
    public Vector3 previousLogPosition;

    //works with logs that wrap (MoveCycle script)
    private float wrapThreshold = 5f;


    void Start()
    {
        // Ducker bobbing animation
        // Sequence seq = DOTween.Sequence();
        // seq.Append(transform.DOMoveY(transform.position.y + 1f, 3f));

        DOTween.SetTweensCapacity(100, 10);
        transform.DOScaleY(0.009f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        // Snap ducker to nearest grid
        targetPosition = new Vector3(Mathf.Round(transform.position.x),
            yOffset,
            Mathf.Round(transform.position.z));
        transform.position = targetPosition;
    }

    void Update()
    {
        //runs functions
        HandleInput();
        MoveTowardsTarget();
        RideLog();
        RotateTowardsMovement();
    }

    void HandleInput()
    {
        // Prevents moving before Ducker reaches correct grid position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Vector3 input = Vector3.zero;

            // Takes player input
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                input = Vector3.forward;
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                input = Vector3.back;
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                input = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                input = Vector3.right;

            // Uses player input to move Ducker
            if (input != Vector3.zero)
            {
                moveDirection = input.normalized;
                targetPosition += input;
                targetPosition.y = yOffset;

                // Clamp target position within boundaries
                targetPosition.x = Mathf.Clamp(targetPosition.x, -59f, -44f);
                targetPosition.z = Mathf.Clamp(targetPosition.z, 53f, 67f);

            }
        }
    }

    //ducker moving animation
    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
    }

    //Ducker rotating animation
    void RotateTowardsMovement()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    //log riding logic
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

            // Add log movement to ducker
            transform.position += new Vector3(logDelta.x, 0, logDelta.z);
            targetPosition += new Vector3(logDelta.x, 0, logDelta.z);

            previousLogPosition = currentLog.position;
        }
    }

    //Check if ducker is on a log
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Log"))
        {
            currentLog = other.transform;
            previousLogPosition = currentLog.position;
        }
    }

    //allows ducker off log
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Log") && other.transform == currentLog)
        {
            currentLog = null;
        }
    }
}
