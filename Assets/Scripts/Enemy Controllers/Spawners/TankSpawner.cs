using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpawner : MonoBehaviour
{
    public GameObject tank;
    private float timer;
    public Transform[] waypoints;
    public GameObject pause;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //This IF statement will check to see if there is a tank currently active
        if (gameObject.transform.childCount < 2)
        {
            //if the Tank is dead for over 3 seconds, a new one will be spawned
            if (timer < 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //A new Tank is spawned at the position of the spawner
                var newTank = Instantiate(tank, gameObject.transform);
                newTank.transform.parent = gameObject.transform;
                //timer reset to 0 whe Tank spawned
                timer = 0;
            }
        }
    }
}
