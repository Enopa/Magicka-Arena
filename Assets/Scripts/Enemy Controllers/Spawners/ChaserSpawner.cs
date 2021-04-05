using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserSpawner : MonoBehaviour
{
    public GameObject chaser;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if there are less than 15 chasers currently active, more will be spawned
        if (GameObject.FindGameObjectsWithTag("chaser").Length < 15)
        {
            //timer so that there is spacing between chaser spawning
            if (timer < 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Instantiate(chaser, gameObject.transform);
                timer = 0;
            }
        }
    }
}
