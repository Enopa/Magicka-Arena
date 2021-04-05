using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClient : MonoBehaviour
{
    public static bool immortal;
    public TMPro.TextMeshProUGUI timer;
    private float timerFloat;
    private int timerInt;
    private int minutes;
    private GameObject player;
    private bool dead = false;
    private GameObject deathViewSurvival;
    private GameObject deathViewHunter;
    private GameObject uI;
    public TMPro.TextMeshProUGUI finalScore;
    public TMPro.TextMeshProUGUI finalTime;
    public GameObject[] playerSpawnPoints;
    public GameObject usePoint;
    private GameObject endGame;
    public TMPro.TextMeshProUGUI highScore;
    public TMPro.TextMeshProUGUI highTime;
    // Start is called before the first frame update
    void Start()
    {
        //if you are not immortal, the timer will for up and therefore start from 0
        if (!immortal)
        {
            timerFloat = 0;
            minutes = 0;
        } else
        //if you are immortal, the timer will start from the top and go down
        {
            timerFloat = 60;
            minutes = 1;
        }
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("1") && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Training"))
        {
            //if the scene is not the main menu or training, then that means a match is taking place and different gameObjects will be used
            player = GameObject.Find("Player");
            deathViewSurvival = GameObject.Find("DeathScreenSurvival");
            deathViewHunter = GameObject.Find("DeathScreenHunter");
            uI = GameObject.Find("In-Game UI");
            deathViewSurvival.SetActive(false);
            deathViewHunter.SetActive(false);   
            endGame = GameObject.Find("EndGame");
            endGame.SetActive(false);
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("1"))
        {
            //if the scene is the main menu, that means that the highscores will need to be updated
            highScore.text = string.Format("Your Highest Score Is: " + PlayerPrefs.GetInt("Score"));
            highTime.text = string.Format("The Longest You've Survived for is " + PlayerPrefs.GetInt("Minutes") + " Minutes and " + PlayerPrefs.GetInt("Seconds") + " Seconds");
            if (PlayerPrefs.GetFloat("Sensitivity") < 0f) 
            {
                PlayerPrefs.SetFloat("Sensitivity", 3f);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //will run if the scene is not the menu
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("1") && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Training"))
        {
            if (!immortal)
            {
                //if you are not dead within survival, the timer will continuosly increase
                if (!dead)
                {
                    timerFloat += Time.deltaTime;
                    timerInt = (int)timerFloat;
                    if (timerInt > 60)
                    {
                        minutes += 1;
                        timerFloat = 0f;
                        timerInt = 0;
                    }
                    timer.text = string.Format(minutes + ":" + timerInt);
                } else
                {
                    if (Input.GetButtonDown("Jump"))
                    {   
                        //if you press space after dying, you are loaded back to the main menu
                        SceneManager.LoadScene("1");
                        Cursor.lockState = CursorLockMode.None;
                    }
                }
            } 
            //otherwise you are playing within the hunter game mode
            else
            {
                if (dead)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        //if you press space after you die, you will respawn
                        respawn();
                    }
                }
                //the time has run out if minutes is equal to -1
                else if (minutes <= -1)
                {
                    //displays the final score to the player at the game over screen
                    finalScore.text = string.Format("Final Score: " + player.GetComponent<Player>().player_score);
                    //this will check is the achieved score is a highscore
                    setHighScore(player.GetComponent<Player>().player_score);
                    endGame.SetActive(true);
                    uI.SetActive(false);
                    player.SetActive(false);
                    if (Input.GetButtonDown("Jump"))
                    {
                        //if the player presses space, they will be taken back to the main menu
                        SceneManager.LoadScene("1");
                    }
                }
                else
                {

                    timerFloat -= Time.deltaTime;
                    timerInt = (int)timerFloat;
                    if (timerInt < 0)
                    {
                        minutes -= 1;
                        timerFloat = 60f;
                        timerInt = 60;
                    }
                    timer.text = string.Format(minutes.ToString() + ":" + timerInt.ToString());
                }
            }
        }
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("1"))
        {
            //if you are in the main menu, your cursor is free to move
            Cursor.lockState = CursorLockMode.None;
        }

    }

    //this function is for the exit button
    public void Quit()
    {
        Application.Quit();
    }
    public void gameMode(bool choice)
    {
        //In Survival Mode, immortal is false
        //In Hunter Mode, immortal is true
        immortal = choice;
    }

    //this is for the map buttons within the main menu
    public void mapChange(string map)
    {
        SceneManager.LoadScene(map);
    }

    public void death()
    {
        player.SetActive(false);
        dead = true;

        //as death is handled differently in each game mode, the death function checks what game mode the player is currently within
        if (!immortal)
        {
            deathViewSurvival.SetActive(true);
            finalTime.text = string.Format("You Survived " + minutes.ToString() + " minutes and " + timerInt.ToString() + " seconds");
            //this will check if the achieved timer is the highest time
            setNewHighTime(minutes, timerInt);

        }
        uI.SetActive(false);
        if (immortal)
        {
            //in hunted more, death is handled lightly
            deathViewHunter.SetActive(true);
        }

    }   
    
    public void respawn()
    {
        //this runs a loop through all of the objects within the spawn points array
        foreach (GameObject spawnPoint in playerSpawnPoints)
        {
            //the spawn point with the lowest enemy count is the spawn point used
            if (spawnPoint.GetComponent<SpawnPoint>().enemyCount <= usePoint.GetComponent<SpawnPoint>().enemyCount)
            {
                usePoint = spawnPoint;
            }
        }
        dead = false;
        player.transform.position = usePoint.transform.position;
        player.SetActive(true);
        deathViewHunter.SetActive(false);
        player.GetComponent<Player>().restore();
        uI.SetActive(true);
    }
    

    //this function will check if the minutes and seconds achieved is the highest
    //it will check the minuted first and then the seconds
    public void setNewHighTime(int mins, int secs)
    {
        if (PlayerPrefs.GetInt("Minutes") == mins)
        {
            if (PlayerPrefs.GetInt("Seconds") < secs)
            {
                PlayerPrefs.SetInt("Seconds", secs);
            }
            if (PlayerPrefs.GetInt("Minutes") < mins)
            {
                PlayerPrefs.SetInt("Minutes", mins);
                PlayerPrefs.SetInt("Seconds", secs);
            }
        }
    }

    //this will check to see if the score achieved was the higher than the one achieved
    public void setHighScore(int score)
    {
        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("Score", score);
        }
    }

}
