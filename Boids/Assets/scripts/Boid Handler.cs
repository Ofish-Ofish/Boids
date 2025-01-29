using UnityEngine;

public class BoidHandler : MonoBehaviour
{
    private GameObject[] boids;

    public float alignmentWeight;
    public float cohesionWeight;
    public float separationWeight;

    public float obsticleAvoidanceWeight;

    public float Maxspeed;

    public float viewLength;
    public float FieldOfView;

    public int BoidCount;
    public float sepetaionDistance;

    public int rayCount;


    [SerializeField] private GameObject boidPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boids = new GameObject[BoidCount];
        for (int i = 0; i < BoidCount; i++)
        {
            boids[i] = Instantiate(boidPrefab, new Vector3(Random.Range(-1.8f, 1.8f), Random.Range(.2f, 1.8f), Random.Range(-1.8f, 1.8f)), Quaternion.identity);

        }

        settingsChanges();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown( KeyCode.Return))
        {
            settingsChanges();
        }

    }

    private void settingsChanges() {
        foreach (GameObject boid in boids)
        {

            BoidBrain boidScript = boid.GetComponent<BoidBrain>();  
            boidScript.alignmentWeight = alignmentWeight;
            boidScript.cohesionWeight = cohesionWeight;
            boidScript.separationWeight = separationWeight;
            boidScript.maxSpeed = Maxspeed;
            boidScript.viewLengh = viewLength;
            boidScript.fieldOfView = FieldOfView;
            boidScript.sepetaionDistance = sepetaionDistance;
            boidScript.obsticleAvoidanceWeight = obsticleAvoidanceWeight;
            boidScript.rayCount = rayCount;
            

        }

    }
}
