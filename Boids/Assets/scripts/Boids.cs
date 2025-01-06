using UnityEngine;
using UnityEngine.Timeline;

public class Boids : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 steer;
    private Transform transform;
    private GameObject[] boids;

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
        foreach (GameObject boid in boids)
        {
            if (boid == gameObject)
            { 
                continue;
            }

            alignment += boid.GetComponent<Boids>().steer;

        }

        // print(alignment);
        if (boids.Length > 2)
            alignment = alignment / (boids.Length -1);
            steer = Vector3.Lerp(steer, alignment, Time.deltaTime);


        transform.position +=  steer * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(steer);
        
    }

    private Vector3 Seperation() {
        return Vector3.zero;
    }



    private Vector3 Cohesion() {
        return Vector3.zero;
    }
}
