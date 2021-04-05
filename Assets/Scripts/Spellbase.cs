using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbase : MonoBehaviour
{
    private Rigidbody spell;
    private Vector3 movement;
    public float spell_speed;
    public float spell_time;
    private float timer;
    public float damage;
    public float cost;

    // Update is called once per frame
    void Update()
    {
        //if the spell is active, a timer is started
        //once this timer has reached the required time, the spell is destroyed
        //ensures spells do no last forever
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
        timer = 0;
        spell = gameObject.GetComponent<Rigidbody>();
        //the movement of the spell will be exactly the variable side_movement within the player
        //NOTE: side_movement is actually front_movement, this is an error that I noticed too late to change within scripting
        movement = Player.side_movement;
        gameObject.transform.parent = null;
        spell.AddForce(movement * spell_speed, ForceMode.Impulse);
    }
}
