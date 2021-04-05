using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePatch : MonoBehaviour
{
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //if the Icepatch is enabled for longer than 5 seconds, the gameObject will be destroyed
        if (timer >= 5)
        {
            Destroy(gameObject);
        }
    }
    public void OnEnable()
    {
        //when the gameobject is created, it is seperated from its parent
        //it also begins the timer from the start
        timer = 0;
        gameObject.transform.parent = null;
    }
}
