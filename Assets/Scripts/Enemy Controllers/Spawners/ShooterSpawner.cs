using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSpawner : MonoBehaviour
{
    public GameObject shooter;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        //if there are less than 10 shooters within the scene, then more will be spawned
        if (GameObject.FindGameObjectsWithTag("shooter").Length < 10)
        {

            if (timer < 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Instantiate(shooter, gameObject.transform);
                timer = 0;
            }
        }
    }
}
