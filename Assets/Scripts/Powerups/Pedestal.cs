using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pedestal : MonoBehaviour
{
    public GameObject[] powerups;
    private int index;
    private float timer;
    public Slider timerbar;
    // Start is called before the first frame update
    void Start()
    {
        //when the pedestal is created, a random index is chosen
        index = Random.Range(0, powerups.Length);
    }

    // Update is called once per frame
    void Update()
    {
      if (!powerups[index].activeSelf)
        {
            //this will display the timer, when it runs out an item will be created
            if (timer >= 5)
            {
                //this sets a random number, therefore a random item spawns
                index = Random.Range(0, powerups.Length);
                powerups[index].SetActive(true);
            }
            timer += Time.deltaTime;
            timerbar.gameObject.SetActive(true);
            timerbar.value = timer;
        } else
        {
            //makes the timer disappear when the item spawns
            timerbar.gameObject.SetActive(false);
            timer = 0;
        }
    }
}
