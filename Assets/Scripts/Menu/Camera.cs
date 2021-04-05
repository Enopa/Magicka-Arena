using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform[] waypoints;
    private int index;
    private int lastIndex;
    private float movement_speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //camera is consistently moving towards its current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, movement_speed);
    }

    public void CameraSet(int location)
    {
        //last index is checked when the camera is moving back from a position
        lastIndex = index;
        index = location;
        if (lastIndex == 3 || lastIndex == 4 || index == 3 || index == 4 || lastIndex == 2 || index == 2 || lastIndex == 6 || index == 6)
        {
            //if the position the camera has to reach is far away, the camera must move faster
            //this ensures the client has a smooth menu transition
            movement_speed = 1.5f;
        }
        else
        {
            movement_speed = .5f;
        }
    }
}



