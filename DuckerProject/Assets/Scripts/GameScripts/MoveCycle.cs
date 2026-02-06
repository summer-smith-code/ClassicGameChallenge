using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{
    public Vector3 direction = Vector3.right;
    public float speed = 3f;
    // size of object
    public int size = 1;

    private Vector3 leftEdge;

    private Vector3 rightEdge;

    // Start is called before the first frame update
    void Start()
    {
        float camZ = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camZ));
        rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, camZ));
        Debug.Log("Left Edge: " + leftEdge + " Right Edge: " + rightEdge);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.x > 0 && (transform.position.x - size) > rightEdge.x)
        {
            Vector3 pos = transform.position;
            pos.x = leftEdge.x - size;
            transform.position = pos;
        }
        else if (direction.x < 0 && (transform.position.x + size) < leftEdge.x)
        {
            Vector3 pos = transform.position;
            pos.x = rightEdge.x + size;
            transform.position = pos;
        } else
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}
