using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    //for the waypoint moving
    private Transform[] waypoints;
    private int x;
    private Vector3 targetPosition;
    public float lookRadius;
    private float timer;
    public GameObject bullet;
    private GameObject player;
    public GameObject pause;

    // Update is called once per frame
    void Update()
    {
        //checks if the player is within range
        if (pause.activeSelf)
        {
            //if the pause menu is active, the tank will not do anything 
        } else if (Vector3.Distance(transform.position, player.transform.position) >= lookRadius)
        {
            //for moving towards the waypoints
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, .1f);
        } else
        {
            shoot();
        }

        //this will check if the enemy has reached the waypoint
        if (Vector3.Distance(transform.position, targetPosition) < .25f)
        {
            if (x >= waypoints.Length - 1)
            {
                //goes back to the beginning of the array
                x = 0;
            }
            else
            {
                //index increased within the array
                x++;
            }    
        }
       
    }
    private void FixedUpdate()
    {
        //the new position of the array is called and used
        targetPosition = new Vector3(waypoints[x].position.x, 1.3f, waypoints[x].position.z);
    }
    public void OnDrawGizmos()
    {
        //helps to visualise the Tank's shoot radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    
    public void shoot()
    {
        //generates enemy shadow attacks at intervals
        if (timer >= 1)
        {
            Instantiate(bullet, gameObject.transform);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public void OnEnable()
    {
        //when the object is created, it will define some of its variables
        //waypoint and pause are taken from the TankSpawner script within their own spawner
        waypoints = gameObject.GetComponentInParent<TankSpawner>().waypoints;
        player = GameObject.Find("Player");
        pause = gameObject.GetComponentInParent<TankSpawner>().pause;
    }
}
