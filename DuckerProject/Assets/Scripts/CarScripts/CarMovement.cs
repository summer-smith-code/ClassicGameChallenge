using UnityEngine;

// Car movement script
public class CarMovement : MonoBehaviour
{
    //set car speed and despawn position
    public float speed = 2f;
    public float despawnX = 20f;

    //moves car forward and despawns it when out of bounds
    void Update()
    {
        //move car forward
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        //despawn car if it goes out of bounds
        if (transform.position.x >= despawnX)
        {
            Destroy(gameObject);
        }
    }
}
