using UnityEngine;

// Resets the car's position when it enters the trigger
public class Lvl2Reset : MonoBehaviour

{
    public Transform resetPoint;
    public float zOffset = 3f;

    //Reset position of the car/log when it enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Car") && !other.CompareTag("Log")) return;

        //Debug.Log("Car triggered reset point: " + other.name);

        // Only modify Z position
        Vector3 newPos = other.transform.position;

        // Add offset
        newPos.z = resetPoint.position.z + zOffset;

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
