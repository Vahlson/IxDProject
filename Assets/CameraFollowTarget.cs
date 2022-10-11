using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{

    [SerializeField] Player player;
    [SerializeField] Transform followTransform;

    public Transform currentCenterWaypoint;
    private Tile currentTile;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        print(player.targetWayPoint.parent.parent.GetComponent<Tile>());
        Tile tile = player.targetWayPoint.parent.parent.GetComponent<Tile>();
        if (tile != null) { currentTile = tile; }
        Transform centerWaypointPos = currentTile?.end[1]?.transform ?? currentTile?.start[1]?.transform;

        if (centerWaypointPos != null)
        {
            currentCenterWaypoint = centerWaypointPos;
        }
        //g = centerWaypointPos?.gameObject;


        Vector3 followTransformSignedForward = new Vector3(Mathf.Abs(followTransform.forward.x), Mathf.Abs(followTransform.forward.y), Mathf.Abs(followTransform.forward.z));
        Vector3 centerWaypointSignedRight = new Vector3(Mathf.Abs(currentCenterWaypoint.right.x), Mathf.Abs(currentCenterWaypoint.right.y), Mathf.Abs(currentCenterWaypoint.right.z));

        print("THETAG: " + currentTile?.tag);

        if ((currentTile.CompareTag("Left") || currentTile.CompareTag("Right")))
        {
            //if(currentTile.CompareTag("Left"))
            centerWaypointSignedRight = new Vector3(Mathf.Abs(currentTile.transform.forward.x), Mathf.Abs(currentTile.transform.forward.y), Mathf.Abs(currentTile.transform.forward.z));
            transform.position = currentCenterWaypoint.position;
        }
        else
        {
            transform.position = Vector3.Scale(followTransform.position, followTransformSignedForward) + Vector3.Scale(currentCenterWaypoint.position, centerWaypointSignedRight);
        }







        print("forward: " + Vector3.Scale(followTransform.position, followTransformSignedForward) + "right: " + Vector3.Scale(currentCenterWaypoint.position, centerWaypointSignedRight));
        transform.forward = currentCenterWaypoint.forward;
        //transform.position = transformToFollow.position;
    }
}
