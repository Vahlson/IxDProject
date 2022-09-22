using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockadeType{
    Red,
    Green,
    Blue,
    None
}

public class BlockadeScript : MonoBehaviour
{
    private BlockadeType None;

    private List<GameObject> obstacles;
    
    [SerializeField]
    private GameObject obstaclePrefab;
    private const int maxWalls = 3;

    private Collider waypointCollider; 
    private Mesh obstacleMesh;
    private Collider obstacleCollider;

    void Awake(){
        
        print("yo");
        waypointCollider = TryGetComponent(out MeshCollider meshCollider)? waypointCollider = meshCollider : null;

        int nWalls = Random.Range(0, maxWalls);
        Vector3 parentPosition = transform.position;
        Vector3 parentExtents = waypointCollider.bounds.extents;
        Vector3 parentSize = waypointCollider.bounds.extents * 2;

        float yOffset = obstaclePrefab.transform.localScale.y / 2;
        obstacleMesh = obstaclePrefab.TryGetComponent(out MeshFilter meshFilter) ? meshFilter.sharedMesh : null;
        print(obstacleMesh);
        Vector3 obstacleExtents = obstacleMesh?.bounds.extents ?? Vector3.zero;
        print(obstacleMesh?.bounds);

        //Get the collider
        obstacleCollider = obstaclePrefab.TryGetComponent(out BoxCollider collider) ? collider : null;

        List<Vector3> ObstacleSpawnPoints = new List<Vector3>();
        Vector3 start = new Vector3(parentPosition.x, parentPosition.y + yOffset, parentPosition.z - parentExtents.z + obstacleExtents.z);
        Vector3 center = new Vector3(parentPosition.x, parentPosition.y + yOffset, parentPosition.z);
        Vector3 end = new Vector3(parentPosition.x, parentPosition.y + yOffset, parentPosition.z + parentExtents.z - obstacleExtents.z);
        ObstacleSpawnPoints.Add(start);
        ObstacleSpawnPoints.Add(center);
        ObstacleSpawnPoints.Add(end);
        
        //Create walls
        obstacles = new List<GameObject>();
        for(int i = 0; i < nWalls ; i ++){
            
            
            Vector3 spawnPoint = ObstacleSpawnPoints[Random.Range(0,maxWalls)];
            ObstacleSpawnPoints.Remove(spawnPoint);

            //Instantiate obstacle and change size to fit parent block.
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint, Quaternion.identity);

            //Change width to fit parent.
            //TODO, Change this to only scale the collider 
            obstacle.transform.localScale = new Vector3(parentSize.x,obstacle.transform.localScale.y,obstacle.transform.localScale.z);

            obstacles.Add(obstacle);

        }

        print("heyo");
    }
    // Start is called before the first frame update
    void Start()
    {
    
    }

    void FixedUpdate(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
