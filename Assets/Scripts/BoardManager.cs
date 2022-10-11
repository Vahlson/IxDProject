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

    //Creates a baseline number of forward tiles in a row so there are at least a couple before a turn.
    private int nForwardTilesInRow = 0;
    [SerializeField] public int minForwardTilesBeforeTurn = 2;
    [SerializeField]
    GameObject badguyGO;

    void Awake()
    {
        GameManager.Instance.gameState = GameState.ongoing;
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
        // setBadGuyWaypoint();
    }
    // void setBadGuysWaypoint(Waypoint oldWp)
    // {
    //     Badguy badguy = badguyGO.GetComponent<Badguy>();
    //     if (badguy.targetWayPoint != oldWp.transform)
    //     {
    //         badguy.targetWayPoint = oldWp.transform;

    //     }
    //     else
    //     {
    //         badguy.targetWayPoint = oldWp.next.transform;
    //     }

    // }
    void setPlayerWaypoint()
    {
        if (player.hasReachedTarget())
        {
            Waypoint oldWp = player.targetWayPoint.gameObject.GetComponent<Waypoint>();
            // setBadGuysWaypoint(oldWp);

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

    // void setBadGuyWaypoint()
    // {
    //     Badguy badguy = badguyGO.GetComponent<Badguy>();

    //     if (badguy.hasReachedTarget())
    //     {
    //         Waypoint oldWp = badguy.targetWayPoint.gameObject.GetComponent<Waypoint>();

    //         badguy.targetWayPoint = oldWp.next.transform;

    //         if (oldWp.isEnd)
    //         {
    //             _platforms.Remove(_passedTile);
    //             Destroy(_passedTile);
    //             // getRandomTile();
    //             _passedTile = _platforms[0];
    //         }
    //     }
    // }
    private void initStartTile()
    {
        GameObject startTileGO = Instantiate(_start, transform.position, transform.rotation);
        Tile startTile = startTileGO.GetComponent<Tile>();
        //TODO move forward based on size of waypoint mesh
        transform.position += transform.forward * 15;
        nForwardTilesInRow++;
        GameManager.Instance.IncreaseNTilesSpawned();
        // spawn a ground plane to hide infinity
        /* float offsetFromNew = getTileOffset(startTile);
        float offsetFromOld = getTileOffset(_endTile);
        print(transform.forward);
        transform.position += transform.forward * (offsetFromNew + offsetFromOld);
        print("Offset from old: " + offsetFromOld + "offset from new: " + offsetFromNew); */

        _secondLastTile = startTile;
        _platforms.Add(startTileGO);
        _passedTile = _platforms[0];
        startTile.setStartTile();
        player.targetWayPoint = startTile.getStartWaypoint();
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
        int tempForwardTilesInRow = nForwardTilesInRow;
        string tag = TileTypes.Forward.ToString();
        float tileThreshold = Random.value;
        if (tileThreshold <= 0.1 && nForwardTilesInRow >= minForwardTilesBeforeTurn)
        {
            tag = TileTypes.Left.ToString();
            //Reset forward tiles counter
            nForwardTilesInRow = 0;
        }
        else if (tileThreshold <= 0.2 && nForwardTilesInRow >= minForwardTilesBeforeTurn)
        {
            tag = TileTypes.Right.ToString();
            //Reset forward tiles counter
            nForwardTilesInRow = 0;

        }
        else if (tileThreshold > 0.2)
        {
            tag = TileTypes.Forward.ToString();
            nForwardTilesInRow++;

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
                //print("Forward");
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
            // if position is already taken, get new random tile. RESET and try again.
            if (Vector3.Distance(item.transform.position, transform.position) < 1)
            {
                this.transform.rotation = tempRotation;
                this.transform.position = tempPosition;
                nForwardTilesInRow = tempForwardTilesInRow;
                getRandomTile();
                return;
            }
        }
        _endTile = Instantiate(prefab, tempPosition, tempRotation).GetComponent<Tile>();

        //Increment global counter
        GameManager.Instance.IncreaseNTilesSpawned();

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