using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] platformTypes;
    private Queue<GameObject> _platforms = new Queue<GameObject>();
    private GameObject passed;
    private Player player;
    private GameObject current;

    void Start()
    {
        Tile prev = null;
        player = GameObject.FindObjectOfType<Player>();
        for (int i = 0; i < platformTypes.Length; i++)
        {
            Tile last = Instantiate(platformTypes[i], transform.position, transform.rotation).GetComponent<Tile>();
            print(last.gameObject.name);
            foreach (var element in last.GetComponentsInChildren<MeshRenderer>())
            {
                Color c = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                element.material.color = c;
            }


            switch (last.gameObject.tag)
            {
                case "Left":
                    transform.Rotate(new Vector3(0, -90, 0));
                    // replace with object size.
                    transform.position += transform.forward * 30;
                    break;
                case "Right":
                    transform.Rotate(new Vector3(0, 90, 0));
                    transform.position += transform.forward * 30;
                    break;
                case "Forward":
                    transform.Rotate(new Vector3(0, 0, 0));
                    transform.position += transform.forward * 30;
                    break;
            }
            if (prev != null)
            {
                prev.addNext(last);
            }
            _platforms.Enqueue(last.gameObject);
            prev = last;
        }
        current = _platforms.Dequeue();
        player.targetWayPoint = current.GetComponent<Tile>().start[1].transform;

    }
    void Update()
    {
        if (player.transform.position == player.targetWayPoint.transform.position)
        {
            // _platforms.Enqueue(current);
            // current = _platforms.Dequeue();
            player.targetWayPoint = player.targetWayPoint.gameObject.GetComponent<Waypoint>().next.transform;

        }
    }

    public void RecyclePlatform(GameObject platform)
    {

        // if (passed != null)
        // {
        //     passed.transform.position = transform.position;
        //     placeWaypoint(passed.GetComponent<Waypoint>().gameObject, _directions[Random.Range(0, _directions.Length)]);
        // }
        passed = platform;

    }

    private void placeWaypoint(GameObject gameobject, string tag)
    {
        gameobject.tag = tag;
        Quaternion tempRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
         transform.rotation.eulerAngles.y,
          transform.rotation.eulerAngles.z);
        Vector3 tempPosition = transform.position;


        foreach (var item in _platforms)
        {
            if (item.transform.position == this.transform.position)
            {
                this.transform.rotation = tempRotation;
                this.transform.position = tempPosition;
                // placeWaypoint(gameobject, _directions[Random.Range(0, _directions.Length)]);
                return;
            }
        }
        // player.waypoints.Enqueue(gameobject.transform);
    }
}
