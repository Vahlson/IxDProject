using UnityEngine;
using System.Collections;
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
    private string[] _directions = { "Left", "Right", "Forward" };
    private Tile _endTile = null;
    private Tile _secondLastTile = null;
    private GameObject _passedTile = null;

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        initStartTile();
        for (int i = 0; i < 4; i++)
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
        if (player.targetWayPoint != null && player.transform.position == player.targetWayPoint.transform.position)
        {
            Waypoint wp = player.targetWayPoint.gameObject.GetComponent<Waypoint>();
            if (wp.isEnd)
            {
                player.targetWayPoint = wp.next.transform;
                if (wp.next.transform == null)
                {
                    print("next wp:" + wp.next);
                    print("player wp:" + player.targetWayPoint);

                }
                _platforms.Remove(_passedTile);
                Destroy(_passedTile);
                getRandomTile();
                _passedTile = _platforms[0];
            }
            else
            {
                if (wp.next.transform == null)
                {
                    print("next wp:" + wp.next);
                    print("player wp:" + player.targetWayPoint);

                }
                player.targetWayPoint = wp.next.transform;
            }
        }
    }
    private void initStartTile()
    {
        GameObject startTileGO = Instantiate(_start, transform.position, transform.rotation);
        Tile startTile = startTileGO.GetComponent<Tile>();
        transform.Rotate(new Vector3(0, 0, 0));
        transform.position += transform.forward * 30;
        _secondLastTile = startTile;
        _platforms.Add(startTileGO);
        _passedTile = _platforms[0];
        startTile.setStartTile();
        player.targetWayPoint = startTile.getStartWaypoint(); ;

    }

    private void getRandomTile()
    {
        string tag = _directions[Random.Range(0, _directions.Length)];
        Quaternion tempRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
    transform.rotation.eulerAngles.y,
    transform.rotation.eulerAngles.z);
        Vector3 tempPosition = transform.position;
        GameObject prefab = null;
        // randomize a direction and randomize which of the available tiles for that direction to be used.
        switch (tag)
        {
            case "Left":
                transform.Rotate(new Vector3(0, -90, 0));
                transform.position += transform.forward * 30;
                prefab = leftTiles[Random.Range(0, leftTiles.Count)];
                break;
            case "Right":
                transform.Rotate(new Vector3(0, 90, 0));
                transform.position += transform.forward * 30;
                prefab = rightTiles[Random.Range(0, rightTiles.Count)];
                break;
            case "Forward":
                transform.position += transform.forward * 30;
                prefab = forwardTiles[Random.Range(0, forwardTiles.Count)];
                break;

        }
        // ensures that no two platforms are spawned at the same location.
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
        Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        //set next waypoint for ends of tile
        _secondLastTile.addNext(_endTile);
        //to keep track of all tiles
        _platforms.Add(_endTile.gameObject);
        //to remember tile to set next waypoints for
        _secondLastTile = _endTile;
        return;

    }
}
