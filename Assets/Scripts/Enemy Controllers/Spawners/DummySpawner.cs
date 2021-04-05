using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    public GameObject dummy;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if there isn't a dummy at its spawn point one will be created
        if (gameObject.transform.childCount < 1)
        {
            if (timer < 3)
            {
                timer += Time.deltaTime;
            } else
            {
                var newDummy = Instantiate(dummy, gameObject.transform);
                newDummy.transform.parent = gameObject.transform;
                timer = 0;
            }
        }
    }
}
