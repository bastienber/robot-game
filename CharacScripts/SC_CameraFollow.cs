
using UnityEngine;

public class SC_CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed; //camera speed
    public Vector3 offset; //camera delay
    private float startdistance;

    private void Start()
    {
        startdistance = Vector3.Distance(target.position, transform.position);r
    }


    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        float distance = Vector3.Distance(target.position, transform.position) ; 
        float retard = (distance - startdistance);
        Vector3 smoothedPosition;

        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
