using UnityEngine;
using UnityEngine.Timeline;

public class Boids : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 steer;

    public float inView;
    float maxSpeed = 5.0f;
    private Transform transform;
    private GameObject[] boids;
    private float viewLengh = 10.0f;

    private float fieldOfView = 82.5f;
    
    private Vector3 separation;
    private Vector3 alignment;
    private Vector3 cohesion;
    void Start()
    {

        transform = GetComponent<Transform>();
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        inView = 0;
        alignment = Vector3.zero;
        cohesion = Vector3.zero;

        if(steer.magnitude > maxSpeed) {
            steer = steer.normalized * maxSpeed;
        }


        foreach (GameObject boid in boids)
        {
            if (boid == gameObject)
            { 
                continue;
            }


            if(InFieldOfView(boid)) {
                inView++;
                alignment += boid.GetComponent<Boids>().steer;
                cohesion += boid.transform.position - transform.position;
            }
        }
        if (inView > 0) {
            alignment = alignment / inView;
            cohesion = cohesion / inView;




            steer += Vector3.Slerp(steer, (alignment), 0.1f);
            steer += Vector3.Slerp(steer, (cohesion), 0.1f);
        }

        transform.position +=  steer * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(steer);
        
    }

    private bool InFieldOfView(GameObject boid)
    {

        float dist = Vector3.Distance(boid.transform.position, transform.position);
        if (dist > viewLengh)
        {
            return false;
        }
        float angle = Vector3.Angle(transform.forward, boid.transform.position - transform.position);
        if (angle > fieldOfView)
        {
            return false;
        }
        return true;
    }


}
