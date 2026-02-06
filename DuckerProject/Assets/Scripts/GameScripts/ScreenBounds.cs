using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    [SerializeField] private Vector3 leftEdge;

    [SerializeField] private Vector3 rightEdge;

    // Start is called before the first frame update
    void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        Debug.Log("Left Edge: " + leftEdge + " Right Edge: " + rightEdge);
    }

    public Vector3 getLeftEdge
    {
        get { return leftEdge; }
    }

    public Vector3 getRightEdge
    {
        get { return rightEdge; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
