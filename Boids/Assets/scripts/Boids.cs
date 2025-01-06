using UnityEngine;
using UnityEngine.Timeline;

public class Boids : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 steer;
    private Transform transform;
    private GameObject[] boids;

    private float viewRadius = 5.0f;
    private float viewLengh = 10.0f;

    private Vector3 separation;
    private Vector3 alignment;
    private Vector3 cohesion;
    void Start()
    {

        transform = GetComponent<Transform>();
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        int inView = 0;
        foreach (GameObject boid in boids)
        {
            if (boid == gameObject)
            { 
                continue;
            }


            if(InFieldOfView(boid)) {
                inView++;
                alignment += boid.GetComponent<Boids>().steer;
                alignment += boid.GetComponent<Boids>().steer;
            }
        }

        print("inView: " + inView);
        if (inView > 1) {
            alignment = alignment / inView;
            steer = Vector3.Lerp(steer, alignment, Time.deltaTime);
        }

        transform.position +=  steer * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(steer);
        
    }

    private bool InFieldOfView(GameObject boid)
    {
        Vector3 coneStartingPoint = transform.position;
        Vector3 coneBaseCenter = transform.position + transform.forward * viewLengh;
        Vector3 normalizedConeDir = Vector3.Normalize(coneBaseCenter - coneStartingPoint);
        Vector3 pointToaxis = boid.transform.position - coneStartingPoint;
        float Projection = Vector3.Dot(pointToaxis, normalizedConeDir);

        print("Projection: " + Projection);


        if (Projection < 0 || Projection > viewLengh)
        {
            return false;
        }
        float radicalDistance = Vector3.Magnitude(pointToaxis - Projection * normalizedConeDir);
        float projectionRadius = (viewRadius/viewLengh) * Projection;

        print("radicalDistance: " + radicalDistance);
        print("projectionRadius: " + projectionRadius);

        if (radicalDistance < projectionRadius)
        {
            return true;
        }
        return false;
    }


}
