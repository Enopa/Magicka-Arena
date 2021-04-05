using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Shooter : MonoBehaviour
{
    private Transform player;
    public float lookRadius;
    public GameObject bullet;
    private float timer;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) >= lookRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(gameObject.transform.position);
            shoot();
        }
        transform.LookAt(player.transform.position);
    }

    public void OnDrawGizmos()
    {
        //this draws a small sphere around the enemy within the editor so I can visualise the shooter's radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    //when the player is within the radius of the enemy, this function will be repeated
    //a spell will be instantiated from the enemy when the timer is reset, this is every 1 second here
    public void shoot()
    {
        //when the enemy is close to the player, this function will run
        //it instantiates an enemy shot at regular intervals
        //the shots have their own scripts that cause them to move
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
}
