using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> leftTiles;
    [SerializeField]
    private List<GameObject> rightTiles;
    [SerializeField]
    private List<GameObject> forwardTiles;
    [SerializeField]
    private GameObject _start;
    private List<GameObject> _platforms = new List<GameObject>();
    private Player player;

    private Tile _endTile = null;
    private Tile _secondLastTile = null;
    private GameObject _passedTile = null;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        initStartTile();
        for (int i = 0; i < 7; i++)
        {
            getRandomTile();
        }
    }
    void Update()
    {
        setPlayerWaypoint();
    }
    void setPlayerWaypoint()
    {
        if (player.hasReachedTarget())
        {
            Waypoint oldWp = player.targetWayPoint.gameObject.GetComponent<Waypoint>();
            player.targetWayPoint = oldWp.next.transform;

            if (oldWp.isEnd)
            {
                _platforms.Remove(_passedTile);
                Destroy(_passedTile);
                getRandomTile();
                _passedTile = _platforms[0];
            }
        }
    }
    private void initStartTile()
    {
        GameObject startTileGO = Instantiate(_start, transform.position, transform.rotation);
        Tile startTile = startTileGO.GetComponent<Tile>();
        //TODO move forward based on size of waypoint mesh
        transform.position += transform.forward * 15;
        /* float offsetFromNew = getTileOffset(startTile);
        float offsetFromOld = getTileOffset(_endTile);
        print(transform.forward);
        transform.position += transform.forward * (offsetFromNew + offsetFromOld);
        print("Offset from old: " + offsetFromOld + "offset from new: " + offsetFromNew); */

        _secondLastTile = startTile;
        _platforms.Add(startTileGO);
        _passedTile = _platforms[0];
        startTile.setStartTile();
        player.targetWayPoint = startTile.getStartWaypoint(); ;

    }

    private float getTileOffset(Tile tile)
    {
        float offset = 0;
        Waypoint? exampleWaypoint = tile?.start[0].GetComponent<Waypoint>();
        if (exampleWaypoint != null)
        {
            MeshRenderer? waypointCollider = exampleWaypoint.TryGetComponent(out MeshRenderer meshCollider) ? waypointCollider = meshCollider : null;
            //print(exampleWaypoint);
            if (waypointCollider != null)
            {
                Vector3 waypointBounds = waypointCollider.bounds.extents;
                print("parent size x: " + waypointBounds.x + " parent size z: " + waypointBounds.z);


                //Max since the longest dimension will be forward.
                offset = 3 * Mathf.Max(waypointBounds.x, waypointBounds.z);

            }

        }

        return offset;
    }

    private void getRandomTile()
    {
        string tag = TileTypes.Forward.ToString();
        float tileThreshold = Random.value;
        if (tileThreshold <= 0.1)
        {
            tag = TileTypes.Left.ToString();
        }
        else if (tileThreshold <= 0.2)
        {
            tag = TileTypes.Right.ToString();

        }
        else if (tileThreshold > 0.2)
        {
            tag = TileTypes.Forward.ToString();

        }

        Quaternion tempRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
    transform.rotation.eulerAngles.y,
    transform.rotation.eulerAngles.z);
        Vector3 tempPosition = transform.position;
        GameObject prefab = null;
        Vector3 offset = Vector3.zero;
        // randomize a direction and randomize which of the available tiles for that direction to be used.
        switch (tag)
        {

            case "Left":
                transform.Rotate(new Vector3(0, -90, 0));
                print("Left");
                transform.position += transform.forward * 15;
                prefab = leftTiles[Random.Range(0, leftTiles.Count)];


                /* if (prefab.TryGetComponent(out Tile leftTile))
                {
                    float offsetFromNew = getTileOffset(leftTile);
                    float offsetFromOld = getTileOffset(_endTile);
                    print(transform.forward);
                    transform.position += transform.forward * (offsetFromNew + offsetFromOld);
                    print("transform pos:" + transform.position);
                    print("Offset from old: " + offsetFromOld + "offset from new: " + offsetFromNew);

                } */


                break;
            case "Right":
                print("Right");
                transform.Rotate(new Vector3(0, 90, 0));
                transform.position += transform.forward * 15;
                prefab = rightTiles[Random.Range(0, rightTiles.Count)];

                /* if (prefab.TryGetComponent(out Tile rightTile))
                {
                    float offsetFromNew = getTileOffset(rightTile);
                    float offsetFromOld = getTileOffset(_endTile);
                    print(transform.forward);
                    transform.position += transform.forward * (offsetFromNew + offsetFromOld);
                    print("Offset from old: " + offsetFromOld + "offset from new: " + offsetFromNew);
                } */
                break;
            case "Forward":
                print("Forward");
                transform.position += transform.forward * 15;
                prefab = forwardTiles[Random.Range(0, forwardTiles.Count)];

                /* if (prefab.TryGetComponent(out Tile forwardTile))
                {
                    float offsetFromNew = getTileOffset(forwardTile);
                    float offsetFromOld = getTileOffset(_endTile);
                    print(transform.forward);
                    transform.position += transform.forward * (offsetFromNew + offsetFromOld);
                    print("Offset from old: " + offsetFromOld + "offset from new: " + offsetFromNew);
                } */
                break;

        }



        // ensures that no two platforms are spawned at the same location assuming there are less than 6 platforms.
        foreach (var item in _platforms)
        {
            // if position is already taken, get new random tile.
            if (Vector3.Distance(item.transform.position, transform.position) < 1)
            {
                this.transform.rotation = tempRotation;
                this.transform.position = tempPosition;
                getRandomTile();
                return;
            }
        }
        _endTile = Instantiate(prefab, tempPosition, tempRotation).GetComponent<Tile>();

        //set next waypoint for ends of tile
        _secondLastTile.addNext(_endTile);
        //to keep track of all tiles
        _platforms.Add(_endTile.gameObject);
        //to remember tile to set next waypoints for
        _secondLastTile = _endTile;
        return;

    }
}
enum TileTypes
{
    Left, Right, Forward
}