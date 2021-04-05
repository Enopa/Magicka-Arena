using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    private Rigidbody spell;
    private Vector3 movement;
    public float spell_speed;
    public float spell_time;
    private float timer;
    public float damage;
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        //if the enemy shot is active for too long, it will be destroyed
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;
        }
        if (timer > spell_time)
        {
            Destroy(gameObject);
        }

    }
    public void OnEnable()
    {
        player = GameObject.Find("Player");
        timer = 0;
        gameObject.transform.parent = null;
        spell = gameObject.GetComponent<Rigidbody>();
        movement = player.transform.position - gameObject.transform.position;
        
        //this causes the shadow to move as soon as it is spawned, heading instantly in the players direction
        spell.AddForce(movement * spell_speed, ForceMode.Impulse);
    }
    public void OnTriggerEnter(Collider other)
    {
        //if the collider is not a Tank, Shooter, Chaser or SpawnPoint, the object will not be destroyed
        if (other.tag != "tank" && other.tag != "shooter" && other.gameObject.layer != 10 && other.gameObject.layer != 12)  
        {
            Destroy(gameObject);
        }
        //if the shadow hits the player, the damage function is run
        if (other.tag == "player")
        {
            player.GetComponent<Player>().takeDamage(damage);
        }
    }
}
