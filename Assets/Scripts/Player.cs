using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody body;
    private GameObject front;
    private GameObject side;
    [Range(0f, 1f)]
    public float forward_speed;
    public float side_speed;


    //some float values
    public float sensitivity;
    private float sens_store;
    private float mana_timer;
    private bool alertmana;
    private float forward_store;
    private float side_store;
    public int player_score;


    public static Vector3 side_movement;
    private Vector3 front_movement;

    //Retrieved GameObjects
    private GameObject menu;
    private Slider health_bar;
    private Slider mana_bar;
    private GameObject noMana;
    private GameObject pause_menu;
    public TMPro.TextMeshProUGUI scoreText;
    private GameObject shieldIcon;
    private GameObject game;


    //----------SPELLS----------------
    //as these remain inactive within the scene, i must make them public
    public GameObject[] spells;
    private int spell_1 = 0;
    private int spell_2 = 1;


    // Start is called before the first frame update
    void Start()
    {
        //this section is used to define all the gameobjects
        body = gameObject.GetComponent<Rigidbody>();
        front = GameObject.Find("Front");
        side = GameObject.Find("Side");
        menu = GameObject.Find("Spells");
        health_bar = GameObject.Find("Healthbar").GetComponent<Slider>();
        mana_bar = GameObject.Find("Mana").GetComponent<Slider>();
        noMana = GameObject.Find("LowMana");
        pause_menu = GameObject.Find("Pause");
        shieldIcon = GameObject.Find("Shield");
        game = GameObject.Find("Game");

        //Here I am setting all of the gameObjects that have been retrieved to false. These are all UI features that the player will need
        //They are active when the scene starts so that the script can retrieve their gameObjects
        //I then set them to false as they shouldnt be active at the beginning of the scene
        shieldIcon.SetActive(false);
        menu.SetActive(false);
        pause_menu.SetActive(false);
        noMana.SetActive(false);
        forward_store = forward_speed;
        side_store = side_speed;
        sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        sens_store = sensitivity;

    }

    // Update is called once per frame
    void Update()
    {
        //if the HealthBar value reaches 0, and death function is run within the GameClient script
        if (health_bar.value <= 0f)
        {
            game.GetComponent<GameClient>().death();
        }
        //----------FIRST PERSON CAMERA--------------
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);

        //-------------MOVEMENT-------------------

        //defining the movement vectors
        side_movement = side.transform.position - gameObject.transform.position;
        front_movement = front.transform.position - gameObject.transform.position;
        //making the player move when a button is pressed
        transform.position += front_movement * Input.GetAxisRaw("Horizontal") * forward_speed;
        //using velocity instead of transform.position to ensure they do not clash
        body.velocity = side_movement * Input.GetAxisRaw("Vertical") * side_speed;

        mana_timer -= Time.deltaTime;

        //this checks to see if the pause or spell menu is active
        //the player is unable to shoot if the pause or spell menu is active
        if (!menu.activeSelf && !pause_menu.activeSelf)
        {
            //fire1 is the left mouse button. This IF statement checks for its input
            if (Input.GetButtonDown("Fire1"))
            {
                //this IF statement checks to see if the player has enough mana
                //if the manabar has less mana than the current cost of the spell, then it is not fired
                if (mana_bar.value >= spells[spell_1].GetComponent<Spellbase>().cost)
                {

                    Instantiate(spells[spell_1], side.transform);
                    //if the player does have enough mana, then the shot if fired, but the cost is deducted from the players mana bar
                    mana_bar.value -= spells[spell_1].GetComponent<Spellbase>().cost;
                    alertmana = false;
                }
                else
                {
                    //this will display a small icon indicating that the player does not have enough mana to cast the current spell
                    alertmana = true;
                    mana_timer = 1f;
                }
            }
            //spell 2
            if (Input.GetButtonDown("Fire2"))
            {
                if (mana_bar.value >= spells[spell_2].GetComponent<Spellbase>().cost)
                {
                    Instantiate(spells[spell_2], side.transform);
                    mana_bar.value -= spells[spell_2].GetComponent<Spellbase>().cost;
                    alertmana = false;
                }
                else
                {
                    alertmana = true;
                    mana_timer = 1f;
                }
            }
            side_speed = side_store;
            forward_speed = forward_store;
            sensitivity = sens_store;
        } else
        {
            //if the pause menu is activated, then the movement values for the player are set to 0, to ensure that they are also unable to move
            side_speed = 0;
            forward_speed = 0;
            sensitivity = 0;
        }

        //this will display the no mana text when you are low
        if (alertmana && mana_timer >= 0)
        {
            noMana.SetActive(true);
        }
        else
        {
            noMana.SetActive(false);
        }
    }
    public void choose1(int s1)
    {
        //when the spell is chosen on the menu, this is the function that is run
        //spell_1 is the index that the player uses to access the spell array. 
        //therefore spell_1 determines what spell is going to be instantiated
        spell_1 = s1;
        menu.SetActive(false);
    }

    //same function but for the second spell
    public void choose2(int s2)
    {
        spell_2 = s2;
        menu.SetActive(false);
    }

    //this function  is run when the value of the sensitivity slider is changed
    public void sensitivityChanged()
    {
        sensitivity = GameObject.Find("Sensitivity").GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        //this will store the value of sensitivity within the Unity files
        sens_store = sensitivity;
    }

    //if the player takes damage, this script is run
    public void takeDamage(float damage)
    {
        //if the shield is active after using the rock spell, the player will take no damage
        if (shieldIcon.activeSelf)
        {
            damage = 0;
            //however the shield is set to disabled as soon as you take a hit
            shieldIcon.SetActive(false);
        }
        //if you have no shield, you take damage
        health_bar.value -= damage;
    }

    //on a trigger collision, this function runs
    private void OnTriggerEnter(Collider other)
    {
        //function will check the tag of the object that caused the collision
        switch (other.tag)
        {
            //if it has the tag health, the player gains health
            case "Health":
                health_bar.value += 40;
                other.gameObject.SetActive(false);
                break;
            //if it has the tag mana, the player gains mana
            case "Mana":
                mana_bar.value += 40;
                other.gameObject.SetActive(false);
                break;
            //if it has the tag icepatch, the player's speed is doubled
            case "IcePatch":
                side_store *= 2;
                forward_store *= 2;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //when you leave the icepatch, your speed is halved, returning it to its normal value
        if (other.tag == "IcePatch")
        {
            side_store /= 2;
            forward_store /= 2;
        }
    }

    //When the player recieves points, this function is run
    public void getScore(int score)
    {
        player_score += score;
        scoreText.text = string.Format("{0}", player_score);
    }

    //this is for the grass & thunder spells, will be called when the spell hits the enemy
    public void getHealth()
    {
        health_bar.value += 10;
    }
    public void getMana(int manaGain)
    {
        mana_bar.value += manaGain;
    }

    public void restore()
    {
        health_bar.value = health_bar.maxValue;
        mana_bar.value = mana_bar.maxValue;
    }

    //below is the script for the shield
    public void shield()
    {
        shieldIcon.SetActive(true);
    }

}
