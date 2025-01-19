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
    private Vector3 obsticleAvoidance;
    [HideInInspector] public float alignmentWeight;
    [HideInInspector] public float cohesionWeight;
    [HideInInspector] public float separationWeight;
    [HideInInspector] public float obsticleAvoidanceWeight;

    [HideInInspector] public int rayCount;

    [HideInInspector] public float sphereColliderRadius;


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
        separation = Vector3.zero;
        obsticleAvoidance = Vector3.zero;

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
            // steer = Vector3.Slerp(steer, cohesion, Time.deltaTime);
            // steer = Vector3.Slerp(steer, -separation, Time.deltaTime);
        }

        obsticleAvoidance = ObsticleAvoidance();
        Vector3 weigtedForces = alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight + obsticleAvoidance * obsticleAvoidanceWeight; 



        weigtedForces = weigtedForces.normalized * maxSpeed;

        

        if (weigtedForces == Vector3.zero)
        {
            weigtedForces = steer;
        }
        // Debug.DrawRay(transform.position, weigtedForces, Color.red);

        steer = Vector3.Lerp(steer, weigtedForces, Time.deltaTime);

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
        // Ray ray = new Ray(transform.position, transform.forward * viewLengh); 
        // Debug.DrawRay(ray.origin, ray.direction * viewLengh, Color.green);
        if(Physics.SphereCast(transform.position, sphereColliderRadius, transform.forward, out RaycastHit hit, viewLengh + sphereColliderRadius))
        {
            Debug.Log(hit.collider.gameObject.name);
            float bestAngle = 360;
            float ExtremeAngle = 0;
            Vector3 bestDirection = Vector3.zero;
            Vector3 ExtremeDirection = Vector3.zero;

            for (int i = 0; i < rayCount; i++)
            {
                float indices = i + .5f;
                float phi = Mathf.Acos(1-2*indices/rayCount);
                float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * indices;

                Vector3 normalRay =new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi));

                Physics.Raycast(transform.position, normalRay, out RaycastHit hit2, viewLengh);
                // Debug.DrawRay(transform.position, normalRay * viewLengh, Color.blue);
                if (Vector3.Angle(transform.forward, normalRay) > fieldOfView)
                {
                    Debug.DrawRay(transform.position, normalRay * viewLengh, Color.red);
                    continue;
                }
                if (Vector3.Angle(transform.forward, normalRay) > ExtremeAngle)
                {
                    Debug.DrawRay(transform.position, normalRay * viewLengh, Color.blue);
                    ExtremeDirection = normalRay;
                    ExtremeAngle = Vector3.Angle(transform.forward, normalRay);
                }
                if (hit2.collider != hit.collider && Vector3.Angle(transform.forward, normalRay) < bestAngle)
                {
                    Debug.DrawRay(transform.position, normalRay * viewLengh, Color.green);
                    bestDirection = normalRay;
                    bestAngle = Vector3.Angle(transform.forward, normalRay);

                }

            }
            if (bestDirection != Vector3.zero)
            {
                Debug.DrawRay(transform.position, bestDirection *  viewLengh, Color.black);
                return bestDirection;
            }
            Debug.DrawRay(transform.position, ExtremeDirection * viewLengh, Color.yellow);
            return ExtremeDirection ;
        }
        return Vector3.zero;
    }




}
