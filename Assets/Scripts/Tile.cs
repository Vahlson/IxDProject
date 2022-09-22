using UnityEngine;
class Tile : MonoBehaviour
{
    [SerializeField]
    public GameObject[] start;
    [SerializeField]
    private GameObject[] end;
    public void addNext(Tile tile)
    {
        foreach (var waypoint in end)
        {
            foreach (var item in tile.start)
            {
                if (waypoint.CompareTag(item.tag))
                {
                    Waypoint wp = waypoint.GetComponent<Waypoint>();
                    wp.setNext(item);
                }
            }
        }
    }
    public void setStartTile()
    {
        foreach (var item in end)
        {
            item.GetComponent<Waypoint>().isEnd = false;
        }
    }
    public Transform getStartWaypoint()
    {
        return start[1].transform;
    }
}