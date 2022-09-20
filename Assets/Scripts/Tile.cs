using UnityEngine;
class Tile : MonoBehaviour
{
    [SerializeField]
    public GameObject[] start;
    [SerializeField]
    GameObject[] end;
    public void addNext(Tile tile)
    {
        foreach (var waypoint in end)
        {
            foreach (var item in tile.start)
            {
                if (waypoint.CompareTag(item.tag))
                {
                    waypoint.GetComponent<Waypoint>().next = item;
                }
            }
        }
    }
}