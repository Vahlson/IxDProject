using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockadeType
{
    Up,
    Down,
    Middle,
    None
}

public class BlockadeSpawner : MonoBehaviour
{
    private BlockadeType None;

    private List<GameObject> obstacles;

    //[SerializeField]
    //private GameObject obstaclePrefab;

    [SerializeField]
    private List<GameObject> lowObstaclePrefabs;
    [SerializeField]
    private List<GameObject> midObstaclePrefabs;
    [SerializeField]
    private List<GameObject> highObstaclePrefabs;

    [SerializeField]
    private float blockadeSpawnChance = 0.2f;

    [SerializeField]
    private GameObject startSpawnPoint;
    [SerializeField]
    private GameObject middleSpawnPoint;
    [SerializeField]
    private GameObject endSpawnPoint;

    private Collider waypointCollider;
    private Waypoint waypointController;

    void Awake()
    {

        waypointController = TryGetComponent(out Waypoint w) ? waypointController = w : null;
        waypointCollider = TryGetComponent(out MeshCollider meshCollider) ? waypointCollider = meshCollider : null;

        /*  Vector3 parentPosition = transform.position;
         Vector3 parentExtents = waypointCollider.bounds.extents;
         Vector3 parentSize = waypointCollider.bounds.extents * 2;

         float yOffset = obstaclePrefab.transform.localScale.y / 2;
         obstacleMesh = obstaclePrefab.TryGetComponent(out MeshFilter meshFilter) ? meshFilter.sharedMesh : null;
         Vector3 obstacleExtents = obstacleMesh?.bounds.extents ?? Vector3.zero;

         //Get the collider
         obstacleCollider = obstaclePrefab.TryGetComponent(out BoxCollider collider) ? collider : null; */

        //TODO Make more general and dependent on max walls
        List<Transform> obstacleSpawnPoints = new List<Transform>();
        if (startSpawnPoint.activeInHierarchy) obstacleSpawnPoints.Add(startSpawnPoint.transform);
        if (middleSpawnPoint.activeInHierarchy) obstacleSpawnPoints.Add(middleSpawnPoint.transform);
        if (endSpawnPoint.activeInHierarchy && nextStartIsFree()) obstacleSpawnPoints.Add(endSpawnPoint.transform);

        //Create walls
        obstacles = new List<GameObject>();
        foreach (Transform spawnPoint in obstacleSpawnPoints)
        {
            bool shouldSpawn = Random.Range(0f, 1f) < blockadeSpawnChance;
            if (shouldSpawn)
            {
                List<GameObject> obstaclePrefabs = new List<GameObject>();

                int type = Random.Range(0, 3);
                switch (type)
                {
                    case 0:
                        obstaclePrefabs = lowObstaclePrefabs;
                        break;
                    case 1:
                        obstaclePrefabs = midObstaclePrefabs;
                        break;
                    case 2:
                        obstaclePrefabs = highObstaclePrefabs;
                        break;
                    default:
                        obstaclePrefabs = midObstaclePrefabs;
                        break;
                }

                //Choose which blockade to spawn
                GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];

                GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint.position, spawnPoint.rotation);
                obstacle.transform.parent = spawnPoint;

                //Change width to fit parent.
                //TODO, Change this to only scale the collider 
                //obstacle.transform.localScale = new Vector3(parentSize.x,obstacle.transform.localScale.y,obstacle.transform.localScale.z);

                obstacles.Add(obstacle);
            }

        }

    }
    bool nextStartIsFree()
    {
        BlockadeSpawner nextBlockadeScript = null;
        if (waypointController?.next != null)
        {
            nextBlockadeScript = waypointController.next.TryGetComponent(out BlockadeSpawner b) ? b : null;
        }
        GameObject nextWaypointStartSpawnPoint = nextBlockadeScript?.startSpawnPoint;

        return nextWaypointStartSpawnPoint?.transform.childCount <= 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
