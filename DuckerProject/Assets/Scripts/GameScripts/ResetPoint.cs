using UnityEngine;

// Resets the car's position when it enters the trigger
public class ResetPoint : MonoBehaviour

{
    public Transform resetPoint;
    public float xOffset = 2f;

    //Reset position of the car when it enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car")) return;

        Debug.Log("Car triggered reset point: " + other.name);

        // Only modify X position
        Vector3 newPos = other.transform.position;

        // Add offset
        newPos.x = resetPoint.position.x + xOffset;

        Rigidbody rb = other.attachedRigidbody;

        if (rb != null)
        {
            if (!rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            rb.transform.position = newPos;
        }
        else
        {
            other.transform.position = newPos;
        }

    }
}
