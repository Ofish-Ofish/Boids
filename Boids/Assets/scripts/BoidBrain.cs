using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Timeline;

public class BoidBrain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 steer;
    public float inView;
    [HideInInspector] public float maxSpeed;

    private float speed;
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
        speed = maxSpeed;
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), Random.Range(-speed, speed)).normalized * speed;
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
        }

        obsticleAvoidance = ObsticleAvoidance();
        Vector3 weigtedForces = alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight + obsticleAvoidance * obsticleAvoidanceWeight; 



        weigtedForces = weigtedForces.normalized * speed;

        

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
        float bestAngle = 180.0f;
        Vector3 bestDir = transform.forward;

        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, viewLengh);
        if (hit.collider == null)
        {
            return Vector3.zero;
        }

        // Debug.Log("hit: " + hit.collider.gameObject.name);
        // Debug.DrawRay(transform.position, transform.forward * viewLengh, Color.white);
        for (int i = 0; i < rayCount; i++)
        {
            float indices = i + .5f;
            float phi = Mathf.Acos(1-2*indices/rayCount);
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * indices;

            Vector3 normalRay = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi)).normalized;

            if(Physics.Raycast(transform.position, normalRay, out RaycastHit hit2, viewLengh * 2f)) 

            if (hit2.collider != null)
            {
                continue;
            }
            if (Vector3.Angle(transform.forward, normalRay) > fieldOfView)
            {
                continue;
            }
            if (Vector3.Angle(transform.forward, normalRay) < (180 - fieldOfView) *3f)
            {
                continue;
            }

            if (Vector3.Angle(transform.forward, normalRay) < bestAngle)
            {
                bestDir = normalRay;
                bestAngle = Vector3.Angle(transform.forward, normalRay);
            }

            // Debug.DrawRay(transform.position, normalRay * viewLengh * 2f, Color.green);
        }
        // Debug.DrawRay(transform.position, besrtDir * viewLengh * 2f, Color.black);
        return bestDir;
    }
}
