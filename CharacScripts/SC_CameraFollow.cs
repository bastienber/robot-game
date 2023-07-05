
using UnityEngine;

public class SC_CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed; //vitesse du mouvement camera
    public Vector3 offset; //retard camera
    private float startdistance;

    private void Start()
    {
        startdistance = Vector3.Distance(target.position, transform.position); //distance initiale camera-perso 
    }


    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        float distance = Vector3.Distance(target.position, transform.position) ; //distance camera-perso
        float retard = (distance - startdistance);
        Vector3 smoothedPosition;

        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
