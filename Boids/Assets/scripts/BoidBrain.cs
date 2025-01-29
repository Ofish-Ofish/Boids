using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Timeline;

public class BoidBrain : MonoBehaviour
{
    public Vector3 steer;
    private Vector3 separation;
    private Vector3 alignment;
    private Vector3 cohesion;
    private Vector3 obsticleAvoidance;
    public float inView;
    private float speed;
    private float leastTight = 60f; // the least tight a turn the boids make
    private GameObject[] boids;

    // values that are set by sliders/game state
    [HideInInspector] public float maxSpeed;
    [HideInInspector] public float viewLengh;
    [HideInInspector] public float sepetaionDistance;
    [HideInInspector] public float fieldOfView;
    [HideInInspector] public float alignmentWeight;
    [HideInInspector] public float cohesionWeight;
    [HideInInspector] public float separationWeight;
    [HideInInspector] public float obsticleAvoidanceWeight;
    [HideInInspector] public int rayCount;




    void Start()
    {
        speed = maxSpeed; // set speed to max speed to i could theoretically change how fast the birds move
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(-speed, speed), Random.Range(-speed, speed), Random.Range(-speed, speed)).normalized * speed;
    }
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
        
        if (inView > 0) { // avoid devision by 0 error
            alignment = alignment / inView;
            cohesion = cohesion / inView;
        }

        obsticleAvoidance = ObsticleAvoidance();
        Vector3 weigtedForces = alignment * alignmentWeight + cohesion * cohesionWeight + separation * separationWeight + obsticleAvoidance * obsticleAvoidanceWeight; //done to make sure no one force overpowers the rest
        weigtedForces = weigtedForces.normalized * speed; // limit the speed of the boid

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

        //checks if the boid is in the field of view which is a part of a sphere
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



        Vector3 stright = transform.forward;
        Vector3 up = Quaternion.AngleAxis(15, transform.up) * transform.forward;
        Vector3 down = Quaternion.AngleAxis(-15, transform.up) * transform.forward;
        Vector3 left = Quaternion.AngleAxis(-15, transform.right) * transform.forward;
        Vector3 right = Quaternion.AngleAxis(15, transform.right) * transform.forward;

        Physics.Raycast(transform.position, stright, out RaycastHit hit, viewLengh);
        Physics.Raycast(transform.position, up, out RaycastHit hit2, viewLengh);
        Physics.Raycast(transform.position, down, out RaycastHit hit3, viewLengh);
        Physics.Raycast(transform.position, left, out RaycastHit hit4, viewLengh);
        Physics.Raycast(transform.position, right, out RaycastHit hit5, viewLengh);

        // Debug.DrawRay(transform.position, stright * viewLengh, Color.white);
        // Debug.DrawRay(transform.position, up * viewLengh, Color.white);
        // Debug.DrawRay(transform.position, down * viewLengh, Color.white);
        // Debug.DrawRay(transform.position, left * viewLengh, Color.white);
        // Debug.DrawRay(transform.position, right * viewLengh, Color.white);

        if (hit.collider == null && hit2.collider == null && hit3.collider == null && hit4.collider == null && hit5.collider == null)
        {
            return Vector3.zero;
        }


        
        // Debug.DrawRay(transform.position, transform.forward * viewLengh, Color.white);
        // if (hit.collider == null)
        // {
        //     return Vector3.zero;
        // }
        // Debug.Log("hit: " + hit.collider.gameObject.name);

        for (int i = 0; i < rayCount; i++)
        {
            //generate a ray that is appoximently evenly spaced around the boid in a sphere
            float indices = i + .5f;
            float phi = Mathf.Acos(1-2*indices/rayCount);
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * indices;

            Vector3 normalRay = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(phi)).normalized;

            Physics.Raycast(transform.position, normalRay, out RaycastHit blockedPath, viewLengh * 2f);
            if (blockedPath.collider != null)
            {
                continue;
            }
            if (Vector3.Angle(transform.forward, normalRay) > fieldOfView)
            {
                continue;
            }
            if (Vector3.Angle(transform.forward, normalRay) < leastTight)
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
        // Debug.DrawRay(transform.position, bestDir * viewLengh * 2f, Color.green);
        if (bestAngle == 180.0f)
        {
            return Vector3.zero;
        }
        return bestDir;
    }
}
    