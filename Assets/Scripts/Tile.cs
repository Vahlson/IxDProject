using UnityEngine;
class Tile : MonoBehaviour
{
    [SerializeField]
    public GameObject[] start;
    [SerializeField]
    public GameObject[] end;

    [SerializeField] private bool isStartTile = false;

    public void addNext(Tile tile)
    {
        foreach (var waypoint in end)
        {
            foreach (var item in tile.start)
            {
                if (waypoint.CompareTag(item.tag))
                {
                    Waypoint wp = waypoint.GetComponent<Waypoint>();
                    wp.next = item;
                }
            }
        }
    }
    public bool getIsStartTile()
    {
        return isStartTile;
    }

    public void setStartTile()
    {
        foreach (var item in end)
        {
            item.GetComponent<Waypoint>().isEnd = false;
        }
        isStartTile = true;
    }
    public Transform getStartWaypoint()
    {
        return start[1].transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {

            if (other is CapsuleCollider)
            {
                player.keepRunning();
                print("Hit obstacle");
            }

        }

    }
}