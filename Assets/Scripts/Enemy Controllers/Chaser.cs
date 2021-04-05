using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private NavMeshAgent agent;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        //the player is referenced when following them
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //this ensures that the chaser follows the player
        agent.SetDestination(player.transform.position);
        //ensures that the chaser is always looking at the player
        transform.LookAt(player.transform.position);
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if the chaser hits the player, the chaser will be destroyed and the player will take damage
        if (collision.collider.tag == "player")
        {
            player.GetComponent<Player>().takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
