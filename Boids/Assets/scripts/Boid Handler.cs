using UnityEngine;

public class BoidHandler : MonoBehaviour
{
    private GameObject[] boids;

    public float alignmentWeight;
    public float cohesionWeight;
    public float separationWeight;

    public float Maxspeed;

    public float ViewLength;
    public float FieldOfView;

    public int BoidCount;

    public float sepetaionDistance;
    [SerializeField] private GameObject boidPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boids = new GameObject[BoidCount];
        for (int i = 0; i < BoidCount; i++)
        {
            boids[i] = Instantiate(boidPrefab, new Vector3(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(-30, 30)), Quaternion.identity);
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
            boidScript.viewLengh = ViewLength;
            boidScript.fieldOfView = FieldOfView;
            boidScript.sepetaionDistance = sepetaionDistance;
            

        }

    }
}
