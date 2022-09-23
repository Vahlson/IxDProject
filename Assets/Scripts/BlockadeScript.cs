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
    [SerializeField]
    private float blockadeSpawnChance = 0.2f;
    private const int maxWalls = 3;

    private Collider waypointCollider; 
    private Mesh obstacleMesh;
    private Collider obstacleCollider;

    void Awake(){
        
        //print("yo");
        waypointCollider = TryGetComponent(out MeshCollider meshCollider)? waypointCollider = meshCollider : null;

        Vector3 parentPosition = transform.position;
        Vector3 parentExtents = waypointCollider.bounds.extents;
        Vector3 parentSize = waypointCollider.bounds.extents * 2;

        float yOffset = obstaclePrefab.transform.localScale.y / 2;
        obstacleMesh = obstaclePrefab.TryGetComponent(out MeshFilter meshFilter) ? meshFilter.sharedMesh : null;
        //print(obstacleMesh);
        Vector3 obstacleExtents = obstacleMesh?.bounds.extents ?? Vector3.zero;
        //print(obstacleMesh?.bounds);

        //Get the collider
        obstacleCollider = obstaclePrefab.TryGetComponent(out BoxCollider collider) ? collider : null;

        //TODO Make more general and dependent on max walls
        List<Vector3> obstacleSpawnPoints = new List<Vector3>();
        Vector3 start = new Vector3(parentPosition.x, parentPosition.y + yOffset, parentPosition.z);
        start += transform.forward * (- parentExtents.z + obstacleExtents.z);
        Vector3 center = new Vector3(parentPosition.x, parentPosition.y + yOffset, parentPosition.z);
        Vector3 end = new Vector3(parentPosition.x, parentPosition.y + yOffset, parentPosition.z);
        end += transform.forward * (parentExtents.z - obstacleExtents.z);
        obstacleSpawnPoints.Add(start);
        obstacleSpawnPoints.Add(center);
        obstacleSpawnPoints.Add(end);

        

        //Create walls
        obstacles = new List<GameObject>();
        foreach(Vector3 spawnPoint in obstacleSpawnPoints){
            bool shouldSpawn = Random.Range(0f, 1f) < blockadeSpawnChance;
            if(shouldSpawn)
            {
                GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint, transform.rotation);
            obstacle.transform.parent = gameObject.transform;

            //Change width to fit parent.
            //TODO, Change this to only scale the collider 
            //obstacle.transform.localScale = new Vector3(parentSize.x,obstacle.transform.localScale.y,obstacle.transform.localScale.z);

            obstacles.Add(obstacle);
            }
            
        }
        
        /* 
        int nWallsToSpawn = Random.Range(0, maxWalls+1);
        List<int> spawnPointIndices = new List<int>();
        for(int index = 0; index < obstacleSpawnPoints.Count; index++) spawnPointIndices.Add(index);

        for(int i = 0; i < nWallsToSpawn; i ++){

            int index = Random.Range(0,spawnPointIndices.Count);
            int randomSpawnpointIndex = spawnPointIndices[index];
            spawnPointIndices.RemoveAt(index);
            Vector3 spawnPoint = obstacleSpawnPoints[randomSpawnpointIndex];
            
            //Instantiate obstacle and change size to fit parent block.
            GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint, transform.rotation);
            obstacle.transform.parent = gameObject.transform;

            //Change width to fit parent.
            //TODO, Change this to only scale the collider 
            //obstacle.transform.localScale = new Vector3(parentSize.x,obstacle.transform.localScale.y,obstacle.transform.localScale.z);

            obstacles.Add(obstacle);

        } */

        //print("heyo");
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
