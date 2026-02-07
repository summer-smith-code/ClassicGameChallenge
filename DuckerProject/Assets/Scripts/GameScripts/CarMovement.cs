using UnityEngine;

// Moves the car in a specified direction and speed
public class CarMovement : MonoBehaviour
{
    public enum MoveAxis
    {
        X,
        Y,
        Z
    }

    public MoveAxis moveAxis = MoveAxis.X;
    public float direction = 1f;
    public float speed = 1f;

    // Car movement logic
    void Update()
    {
        Vector3 moveDir = Vector3.zero;

        switch (moveAxis)
        {
            case MoveAxis.X:
                moveDir = Vector3.right;
                break;
            case MoveAxis.Y:
                moveDir = Vector3.up;
                break;
            case MoveAxis.Z:
                moveDir = Vector3.forward;
                break;
        }

        // Move the car in the specified direction and speed
        transform.Translate(moveDir * direction * speed * Time.deltaTime, Space.World);
    }
}
