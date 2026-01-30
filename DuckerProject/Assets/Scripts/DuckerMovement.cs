using System.Collections;
using UnityEngine;

public class DuckerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public float moveCooldown = 0.2f;

    private float lastMoveTime;

    void Update()
    {
        // .2 second cooldown between moves
        if (Time.time - lastMoveTime < moveCooldown)
            return;

        //WASD or Arrow Key Input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMove(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMove(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMove(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMove(Vector3.right);
        }
    }

    //Ducker movement logic
    void TryMove(Vector3 direction)
    {
        //Rotate Ducker to direction
        transform.forward = direction;

        //Math to teleport Ducker 1 meter in direction
        Vector3 targetPosition = transform.position + direction * moveDistance;

        // Snap to grid
        targetPosition.x = Mathf.Round(targetPosition.x);
        targetPosition.z = Mathf.Round(targetPosition.z);

        // Teleport Ducker to next grid position
        // transform.position = targetPosition;
        StartCoroutine(Leap(targetPosition));

        //Checks input cooldown
        lastMoveTime = Time.time;


    }
    // Smooth leap animation
    private IEnumerator Leap(Vector3 destination)
    {
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        float leapDuration = 0.125f; // Duration of the leap

        // Smoothly move to the destination over leapDuration
        while (elapsedTime < leapDuration)
        {
            //Interpolate position over time
            transform.position = Vector3.Lerp(startPosition, destination, (elapsedTime / leapDuration));
            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure final position is set
        transform.position = destination;
    }
}
