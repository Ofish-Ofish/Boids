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
    [SerializeField] private float halfBoxLenght;
    [SerializeField] private GameObject boidPrefab;
    void Start()
    {
        BoidCount = GameObject.FindGameObjectWithTag("save").GetComponent<save>().boidCount;
        boids = new GameObject[BoidCount];
        for (int i = 0; i < BoidCount; i++)
        {
            boids[i] = Instantiate(boidPrefab, new Vector3(Random.Range(-(halfBoxLenght - Maxspeed), (halfBoxLenght - Maxspeed)), Random.Range(Maxspeed, (halfBoxLenght - Maxspeed)), Random.Range(-(halfBoxLenght - Maxspeed), (halfBoxLenght - Maxspeed))), Quaternion.identity);

        }
        settingsChanges();
    }

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
