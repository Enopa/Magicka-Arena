using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemybase : MonoBehaviour
{
    [SerializeField]
    private Slider health;
    [SerializeField]
    private int enemy_score;
    private GameObject player;
    private Player player_script;
    private string last_tag;
    private bool burning = false;
    private float burn_timer;
    private float burn_repeat;
    public GameObject icepatch;
    private Vector3 chaserIce;

    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.Find("Player");
        player_script = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //this detects if the HP run out
        //if it does the enemy will die
        if (health.value <= 0)
        {
            Destroy(gameObject);
            player.GetComponent<Player>().getScore(enemy_score);
        }

        //this is the script for if the enemy gets hit by the fire spell
        if (burning == true)
        {
            //burn_repeat checks how many times the enemy has been hurt by the spell
            if (burn_repeat <= 4)
            {
                //burn_timer provides the intervals between damage cycles
                if (burn_timer >= 1)
                {
                    takeDamage(7);
                    burn_timer = 0;
                    burn_repeat += 1;
                }
                else
                {
                    burn_timer += Time.deltaTime;
                }
            }
            else
            {
                burning = false;
                burn_repeat = 0;
            }
        }
    }


    public void takeDamage(float damage)
    {
        health.value -= damage;
    }

    //this retrieves the damage from the spell as the spell hits the player
    public void OnTriggerEnter(Collider other)
    {
        //this stores the last spell that was used to damage the enemy
        last_tag = other.tag;

        //layer 10 is the "Spell" layer, therefore, they take damage here
        if (other.gameObject.layer == 10)
        {
            takeDamage(other.GetComponent<Spellbase>().damage);
        }
        switch (other.tag)
        {
            //fire spell causes burn loop to run
            case "Fire":
                if (!burning)
                {
                    burning = true;
                }
                break;
            //player gains health if the enemy is hit by a grass spell
            case "Grass":
                player_script.getHealth();
                break;
            //the player gains mana when the enemy is hit with a thunder spell
            case "Thunder":
                player_script.getMana(20);
                break;
            //then enemy gets pushed back when this spell is used
            case "Wind":
                //the tank enemy is too heavy to be moved by such a spell, and therefore remains unaffected
                if (gameObject.tag != "tank")
                {
                    gameObject.transform.position -= (gameObject.transform.position - other.transform.position) * 5;
                    Destroy(other.gameObject);
                }
                break;
            //an ice spell causes an icepatch to be formed
            case "Ice":
                //if an icepatch is already active, another will not be spawned
                if (GameObject.FindGameObjectsWithTag("IcePatch").Length < 1)
                {
                    if (tag != "chaser")
                    {
                        Instantiate(icepatch, new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z), Quaternion.identity);
                        icepatch.transform.parent = null;
                    } else
                    {
                        //I had to create this additional IF statement after noticing a bug
                        //The icepatch would spawn in the air if the chaser spell caused it
                       //therefore a specific y coordinate if used here, as well as rotation
                        chaserIce = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z);
                        Instantiate(icepatch, chaserIce, Quaternion.identity);
                    }
                }
                break;
            default:
                break;
        }
    }

    //the destroy function where tags can be checked over for rock and light spells
    public void OnDestroy()
    {
        //when the enemy is destroyed, it checks the last tag it was hit by to see if the rock or light spell will take effect
        if (last_tag == "Rock")
        {
            player_script.shield();
        }
        else if (last_tag == "Light")
        {
            player_script.restore();
        }
    }
    
}


