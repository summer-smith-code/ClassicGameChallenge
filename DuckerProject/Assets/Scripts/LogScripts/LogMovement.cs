using UnityEngine;

// Log movement script
public class LogMovement : MonoBehaviour
{
    //set log speed and despawn position
    public float speed = 2f;
    public float despawnX = 25f;

    //moves log forward and despawns it when out of bounds
    void Update()
    {
        //move log forward
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        //despawn log if it goes out of bounds
        if (transform.position.x >= despawnX)
        {
            Destroy(gameObject);
        }
    }
}
