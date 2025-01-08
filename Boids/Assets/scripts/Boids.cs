using UnityEngine;
using UnityEngine.Timeline;

public class Boids : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 steer;

    public float inView;
    float maxSpeed = 3f;
    private GameObject[] boids;
    private float viewLengh = 20.0f;

    private float fieldOfView = 160.0f;
    
    private Vector3 separation;
    private Vector3 alignment;
    private Vector3 cohesion;
    void Start()
    {
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
        transform.position = new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30));
    }

    // Update is called once per frame
    void Update()
    {
        inView = 0;
        alignment = Vector3.zero;
        cohesion = Vector3.zero;

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
                Vector3 relativePos = boid.transform.position - transform.position;
                separation += 1/(relativePos.magnitude) * relativePos.normalized;
            }
        }
        if (inView > 0) {
            alignment = alignment / inView;
            cohesion = cohesion / inView;
            
            




            steer += Vector3.Slerp(steer, alignment, 0.1f);
            steer += Vector3.Slerp(steer, cohesion, 0.1f);
            steer += Vector3.Slerp(steer, -separation, 0.1f);
        }

        if(steer.magnitude > maxSpeed) {
            steer = steer.normalized * maxSpeed;
        }

        ObsticleAvoidance();

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

    private Vector3 ObsticleAvoidance()
    {
        Ray ray = new Ray(transform.position, transform.forward * viewLengh); 
        Debug.DrawRay(ray.origin, ray.direction * viewLengh, Color.green);
        return Vector3.zero;
    }


}
