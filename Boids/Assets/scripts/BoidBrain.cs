using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Timeline;

public class BoidBrain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 steer;
    public float inView;
    [HideInInspector] public float maxSpeed;
    private GameObject[] boids;
    [HideInInspector] public float viewLengh;

    [HideInInspector] public float sepetaionDistance;

    [HideInInspector] public float fieldOfView;
    
    private Vector3 separation;
    private Vector3 alignment;
    private Vector3 cohesion;
    [HideInInspector] public float alignmentWeight;
    [HideInInspector] public float cohesionWeight;
    [HideInInspector] public float separationWeight;

    [HideInInspector] public int rayCount = 200;


    void Start()
    {
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed)).normalized * maxSpeed;
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
                alignment += boid.GetComponent<BoidBrain>().steer;
                cohesion += boid.transform.position - transform.position;
                Vector3 relativePos = boid.transform.position - transform.position;
                if (relativePos.magnitude < sepetaionDistance) {
                    separation += 1/(relativePos.magnitude) * -relativePos.normalized;
                }
            }

        }
        if (inView > 0) {
            alignment = alignment / inView;
            cohesion = cohesion / inView;
            
            



            Vector3 weigtedForces = alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight;
            weigtedForces = weigtedForces.normalized * maxSpeed;
            steer = Vector3.Slerp(steer, weigtedForces, Time.deltaTime);
            // steer = Vector3.Slerp(steer, cohesion, Time.deltaTime);
            // steer = Vector3.Slerp(steer, -separation, Time.deltaTime);
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
        Vector3[] points = new Vector3[rayCount];

        for (int i = 0; i < rayCount; i++)
        {
            float indices = i + .5f;
            float phi = Mathf.Acos(1-2*indices/rayCount);
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * indices;

            points[i] = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi)) + transform.position;
            Debug.DrawRay(points[i], (points[i] - transform.position) * viewLengh, Color.red);
        }

        Debug.Log(points);

        

        return Vector3.zero;
    }


}
