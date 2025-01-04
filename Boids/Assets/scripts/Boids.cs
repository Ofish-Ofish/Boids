using UnityEngine;
using UnityEngine.Timeline;

public class Boids : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 steer;
    private Transform transform;
    private GameObject[] boids;

    private Vector3 separation;
    private Vector3 alignment;
    private Vector3 cohesion;
    void Start()
    {

        transform = GetComponent<Transform>();
        boids = GameObject.FindGameObjectsWithTag("boid");
        steer = new Vector3(Random.Range(0.0f, 360.0f), 0, Random.Range(0.0f, 360.0f));
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

            // alignment += boid.transform.rotation.eulerAngles;

        }

        // print(alignment);
        // // alignment = alignment / (boids.Length -1);
        // // print(alignment);
        // alignment = Vector3.zero;
    }

    private void FixedUpdate() {
        transform.position +=  transform.up;
        transform.rotation = Quaternion.Euler(steer);
    }

    private Vector3 Seperation() {
        return Vector3.zero;
    }



    private Vector3 Cohesion() {
        return Vector3.zero;
    }
}
